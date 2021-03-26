using IMT.DeveloperConnect.API;
using IMT.DeveloperConnect.API.EFolder.V1.Export;
using IMT.DeveloperConnect.API.Encompass.V3;
using IMT.DeveloperConnect.API.Encompass.V3.Attachments;
using IMT.DeveloperConnect.API.Encompass.V3.Documents;
using IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline;
using IMT.DeveloperConnect.API.Encompass.V3.ResourceLocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EFolderEntityRef = IMT.DeveloperConnect.API.EFolder.V1.EntityReferenceContract;
using EFolderEntityType = IMT.DeveloperConnect.API.EFolder.V1.EntityType;
using EncompassEntityRef = IMT.DeveloperConnect.API.Encompass.V3.EntityReferenceContract;
using EncompassEntityType = IMT.DeveloperConnect.API.Encompass.V3.EntityType;

namespace Exp21.DevConnect.Demo
{
    class Program
    {

        static void Main(string[] args)
        {
            RunDemo().Wait();
            Console.WriteLine("Demo complete!");
            Console.ReadLine();
        }

        private static async Task RunDemo()
        {
            // Login using credentials in App.Config
            using (var session = await Configuration.GetOAuth2Session())
            {
                // Usecase: Investors provides a Purchase Advice document after purchasing a closed loan
                await PurchaseAdvise(session);

                // Usecase: Lenders need to export documents from the eFolder after a loan has closed
                await ExportAttachments(session);

            } // Revoke the OAuth2 token associated with the session
        }


        /// <summary>
        /// Usecase: Investors provides a Purchase Advice document after purchasing a closed loan
        /// 1. Query for loan
        /// 2. Acquire Shared Lock
        /// 3. Create Document
        /// 4. Upload Attachments
        /// 5. Assign Attachments to Document
        /// 6. Release Shared Lock
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private static async Task PurchaseAdvise(IApiSession session)
        {
            // 1. Query for loan
            var loanId = await QueryForLoan(session, Configuration.LoanNumber, Configuration.LoanFolder);

            // 2. Acquire Shared Lock
            using (await AcquireSharedLock(session, loanId))
            {
                // 3. Create Document
                var document = await CreateDocument(session, loanId);

                /** 
                 * We have 2 attachemnts ready to be uploaded in the directory {DIR_OF_THIS_EXE}\res\attachments
                 * "Sample BBT PA.pdf"
                 * "Sample Purchase Advice.pdf" 
                 */
                var attachmentsDirPath = Path.Combine(Directory.GetCurrentDirectory(), @"res\attachments");

                // 4. Upload Attachments
                var attachmentIds = await UploadAttachments(session, loanId, attachmentsDirPath);

                // 5. Assign Attachments to Document
                await AssignAttachmentsToDocument(session, loanId, document.Id, attachmentIds);


            } // 6. Release Shared Lock
        }

        /// <summary>
        /// Usecase: Lenders need to export documents from the eFolder after a loan has closed
        /// 1. Query for Attachments
        /// 2. Create Export Job
        /// 3. Poll for status
        /// 4. Download and save merged file
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private static async Task ExportAttachments(IApiSession session)
        {
            var loanId = await QueryForLoan(session, Configuration.LoanNumber, Configuration.LoanFolder);

            // 1.Query for Attachments
            var attachments = await GetLatestDocumentAttachmentRefs(session, loanId);

            // 2. Create Export Job
            var jobStatus = await CreateExportJob(session, loanId, attachments);

            // 3. Poll for status
            jobStatus = await PollExportStatus(session, jobStatus.JobId);

            // 4. Download and save merged file
            var downloadFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"res\downloads", Path.GetRandomFileName() + ".pdf");
            await DownloadMergedAttachments(session, jobStatus, downloadFilePath);

        }

        private static async Task<string> QueryForLoan(IApiSession session, string loanNumber, string loanFolder)
        {
            var loanPipelineApi = session.GetApi<LoanPipelineApi>();
            var pipelineItems = await loanPipelineApi.QueryLoanPipeline(new LoanPipelineQueryContract()
            {
                Fields = new List<string>()
                {
                    "LoanId"
                },
                Filter = new QueryCriterionContract()
                {
                    Operator = BinaryOperator.And,
                    Terms = new QueryCriterionContract[]
                    {
                        new QueryCriterionContract()
                        {
                            CanonicalName = "Loan.LoanNumber",
                            Value = loanNumber,
                            MatchType = MatchType.Exact
                        },
                        new QueryCriterionContract()
                        {
                            CanonicalName = "Loan.LoanFolder",
                            Value = loanFolder,
                            MatchType = MatchType.Exact
                        }
                    }
                }
            });


            // Get the loanId from the pipeline items
            var loanId = pipelineItems.FirstOrDefault()?.LoanId;

            if (string.IsNullOrEmpty(loanId))
            {
                // Loan not found, exiting the program!!
                Environment.Exit(1);
            }

            return loanId;
        }

        private static Task<IDisposable> AcquireSharedLock(IApiSession session, string loanId)
        {
            var resourceLockApi = session.GetApi<ResourceLocksApi>();
            var resourceLockApiInput = new ResourceLockContract()
            {
                LockType = ResourceLockType.NGSharedLock,
                Resource = new EncompassEntityRef()
                {
                    EntityId = loanId,
                    EntityType = EncompassEntityType.Loan
                }
            };
            return resourceLockApi.CreateResourceLock(resourceLockApiInput);
        }

        private static async Task<DocumentContract> CreateDocument(IApiSession session, string loanId)
        {
            var documentsApi = session.GetApi<DocumentsApi>();
            var documents = await documentsApi.AddDocuments(
                loanId,
                new DocumentContract[]
                {
                    new DocumentContract()
                    {
                        Title = "Purchase Advice",
                        Description = "Purchase Advice Documents"
                    }
                },
                view: "entity");

            return documents.FirstOrDefault();
        }

        private static async Task<FileAttachmentReferenceContract[]> UploadAttachments(IApiSession session, string loanId, string attachmentsDirPath)
        {
            // We will track the attachment ids in this list to link them to the document later
            var attachmentIds = new List<FileAttachmentReferenceContract>();
            foreach (var fileName in Directory.GetFiles(attachmentsDirPath))
            {
                var filePath = Path.Combine(attachmentsDirPath, fileName);
                var fileInfo = new FileInfo(filePath);
                var fileSizeInBytes = fileInfo.Length;
                var contentType = "application/pdf";

                // First we need to make an api call to generate an upload url for our attachment.
                var attachmentUploadUrlData = await CreateAttachmentUploadUrl(session, loanId, fileName, fileSizeInBytes, contentType);

                // We have generated the url successfully, now we will upload the attachment.
                await UploadAttachment(session, attachmentUploadUrlData, fileInfo, contentType);

                // Record the AttachmentId for later use
                attachmentIds.Add(new FileAttachmentReferenceContract()
                {
                    EntityId = attachmentUploadUrlData.AttachmentId,
                    EntityType = EncompassEntityType.Attachment
                });
            }

            return attachmentIds.ToArray();
        }

        private static Task<AttachmentUploadDataContract> CreateAttachmentUploadUrl(IApiSession session, string loanId, string fileName, long fileSizeInBytes, string contentType)
        {
            var attachmentsApi = session.GetApi<AttachmentsApi>();
            return attachmentsApi.GenerateAttachmentUploadUrl(
                loanId,
                new AttachmentUploadInputContract()
                {
                    File = new AttachmentUploadInputContract.FileData()
                    {
                        ContentType = contentType,
                        Name = fileName,
                        Size = fileSizeInBytes
                    },
                    Title = fileName
                });
        }

        private static async Task UploadAttachment(IApiSession session, AttachmentUploadDataContract attachmentUploadUrlData, FileInfo fileInfo, string contentType)
        {
            var attachmentsApi = session.GetApi<AttachmentsApi>();
            await attachmentsApi.UploadAttachment(attachmentUploadUrlData, fileInfo, contentType);
        }

        private static Task AssignAttachmentsToDocument(IApiSession session, string loanId, string documentId, FileAttachmentReferenceContract[] attachmentIds)
        {
            var documentsApi = session.GetApi<DocumentsApi>();
            return documentsApi.LinkAttachmentsToDocument(loanId, documentId, attachmentIds);
        }

        private static async Task<FileAttachmentReferenceContract[]> GetLatestDocumentAttachmentRefs(IApiSession session, string loanId)
        {
            var documentsApi = session.GetApi<DocumentsApi>();

            // Get documents on the loan
            var documents = await documentsApi.GetDocuments(loanId);

            // Get the attachments linked ot the last document - this is the document we just created.
            var attachments = documents.LastOrDefault()?.Attachments;

            if (attachments?.Any() != true)
            {
                // something went wrong - we should not have endedup here
                Environment.Exit(1);
            }

            return attachments;
        }

        private static async Task<ExportJobStatusContract> CreateExportJob(IApiSession session, string loanId, FileAttachmentReferenceContract[] attachments)
        {
            var exportJobsApi = session.GetApi<ExportJobsApi>();

            // Build the payload to fetch documents using eFolder export job api
            var exportJobInput = new ExportJobContract()
            {
                AnnotationSettings = new AnnotationSettingsContract()
                {
                    Visibility = new VisibilityType[] { VisibilityType.Internal, VisibilityType.Private, VisibilityType.Public }
                },
                Entities = attachments.Select(attachment => new EFolderEntityRef()
                {
                    EntityId = attachment.EntityId,
                    EntityType = EFolderEntityType.Attachment
                }).ToArray(),
                Source = new EFolderEntityRef()
                {
                    EntityId = loanId,
                    EntityType = EFolderEntityType.Loan
                }
            };

            return await exportJobsApi.CreateExportJob(exportJobInput);
        }

        private static async Task<ExportJobStatusContract> PollExportStatus(IApiSession session, string jobId)
        {
            var exportJobsApi = session.GetApi<ExportJobsApi>();
            return await exportJobsApi.PollExportStatus(jobId, 30000);
        }

        private static async Task DownloadMergedAttachments(IApiSession session, ExportJobStatusContract jobStatus, string filePath)
        {
            var exportJobsApi = session.GetApi<ExportJobsApi>();
            await exportJobsApi.DownloadExportedFile(jobStatus, filePath);
        }
    }
}
