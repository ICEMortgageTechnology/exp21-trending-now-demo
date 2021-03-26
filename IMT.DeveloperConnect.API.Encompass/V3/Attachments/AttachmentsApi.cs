using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Attachments
{
    public class AttachmentsApi : SessionBasedApiBase
    {
        private const string AttachmentsUrl = "/encompass/v3/loans/{0}/attachments";
        private const string AttachmentUrl = "/encompass/v3/loans/{0}/attachments/{1}";
        private const string AttachmentUploadUrl = "/encompass/v3/loans/{0}/attachmentUploadUrl";
        private const string AttachmentDownloadUrl = "/encompass/v3/loans/{0}/attachmentDownloadUrl";

        public AttachmentsApi(IApiSession session, IApiClient apiClient) : base(session, apiClient) { }

        public async Task<AttachmentUploadDataContract> GenerateAttachmentUploadUrl(string loanId, AttachmentUploadInputContract input)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));
            ArgumentChecks.IsNotNull(input, nameof(input));

            return await ExecuteNewRequest<AttachmentUploadDataContract>(
                string.Format(AttachmentUploadUrl, loanId),
                Method.Post,
                request => request.AddJsonContent(input));
        }

        public async Task<AttachmentUploadDataContract> UploadAttachment(AttachmentUploadDataContract input, FileInfo attachment, string contentType)
        {
            ArgumentChecks.IsNotNull(input, nameof(input));
            ArgumentChecks.IsNotNull(attachment, nameof(attachment));
            ArgumentChecks.IsNotNullOrEmpty(contentType, nameof(contentType));

            return await ExecuteNewRequest<AttachmentUploadDataContract>(
                input.UploadUrl,
                Method.Put,
                request =>
                {
                    request.AddAuthorizationHeader(input.AuthorizationHeader);
                    request.AddFile(attachment.FullName, contentType);
                });
        }

        public Task<FileAttachmentContract> GetAttachments(string loanId)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));

            return ExecuteNewRequest<FileAttachmentContract>(string.Format(AttachmentsUrl, loanId), Method.Get);
        }

        public Task<FileAttachmentContract> GetAttachment(string loanId, string attachmentId)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));
            ArgumentChecks.IsNotNullOrEmpty(attachmentId, nameof(attachmentId));

            return ExecuteNewRequest<FileAttachmentContract>(string.Format(AttachmentUrl, loanId, attachmentId), Method.Get);
        }

        public Task<AttachmentDownloadDataCollection> GetAttachmentDownloadUrl(string loanId, AttachmentDownloadInputCollection input)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));
            ArgumentChecks.IsNotNull(input, nameof(input));

            return ExecuteNewRequest<AttachmentDownloadDataCollection>(string.Format(AttachmentDownloadUrl, loanId), Method.Post, request => request.AddJsonContent(input));
        }

        public async Task DownloadAttachment(AttachmentDownloadUrlDataContract input, string filePath)
        {
            ArgumentChecks.IsNotNull(input, nameof(input));
            ArgumentChecks.IsNotNullOrEmpty(filePath, nameof(filePath));

            var dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (var request = NewRequest(input.Url, Method.Get))
            {
                request.AddAuthorizationHeader(input.AuthorizationHeader);

                using (var response = await Send(request))
                {
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        var responseStream = await response.ReadContentAsStreamAsync();
                        await responseStream.CopyToAsync(fs);
                    }
                }
            }
        }
    }
}
