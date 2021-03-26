using IMT.DeveloperConnect.API.Encompass.V3.ResourceLocks;
using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Documents
{
    public class DocumentsApi : SessionBasedApiBase
    {
        private const string DocumentsUrl = "/encompass/v3/loans/{0}/documents";
        private const string DocumentAttachmentsUrl = "/encompass/v3/loans/{0}/documents/{1}/attachments";

        public DocumentsApi(IApiSession session, IApiClient apiClient) : base(session, apiClient) { }

        public Task<DocumentContract[]> AddDocuments(string loanId, DocumentContract[] documents, string view = null)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));
            ArgumentChecks.IsNotNull(documents, nameof(documents));

            return ExecuteNewRequest<DocumentContract[]>(
                string.Format(DocumentsUrl, loanId),
                Method.Patch,
                request =>
                {
                    request.AddJsonContent(documents);
                    request.AddQueryParameter("action", "add");
                    if (!string.IsNullOrEmpty(view))
                        request.AddQueryParameter(nameof(view), view);
                });
        }

        public Task<DocumentContract[]> GetDocuments(string loanId)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));

            return ExecuteNewRequest<DocumentContract[]>(string.Format(DocumentsUrl, loanId), Method.Get);
        }

        public Task<DocumentContract[]> LinkAttachmentsToDocument(string loanId, string documentId, FileAttachmentReferenceContract[] attachments, string view = null)
        {
            ArgumentChecks.IsNotNullOrEmpty(loanId, nameof(loanId));
            ArgumentChecks.IsNotNullOrEmpty(documentId, nameof(documentId));
            ArgumentChecks.IsNotNull(attachments, nameof(attachments));

            return ExecuteNewRequest<DocumentContract[]>(
                string.Format(DocumentAttachmentsUrl, loanId, documentId),
                Method.Patch,
                request =>
                {
                    request.AddJsonContent(attachments);
                    request.AddQueryParameter("action", "add");
                    if (!string.IsNullOrEmpty(view))
                        request.AddQueryParameter(nameof(view), view);
                });
        }
    }
}
