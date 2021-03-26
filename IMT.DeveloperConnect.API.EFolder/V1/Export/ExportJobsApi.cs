using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.EFolder.V1.Export
{
    public class ExportJobsApi : SessionBasedApiBase
    {
        private const string CreateUrl = "/efolder/v1/exportjobs";
        private const string GetUrl = "/efolder/v1/exportjobs/{0}";

        public ExportJobsApi(IApiSession session, IApiClient apiClient) : base(session, apiClient)
        {
        }

        public async Task<ExportJobStatusContract> CreateExportJob(ExportJobContract input)
        {
            return await ExecuteNewRequest<ExportJobStatusContract>(CreateUrl, Method.Post, request => request.AddJsonContent(input));
        }

        public async Task<ExportJobStatusContract> PollExportStatus(string jobId, long timeoutMilliSecond)
        {
            ExportJobStatusContract jobStatus = null;
            Stopwatch sw = new Stopwatch();
            while (jobStatus == null || string.Equals(jobStatus.Status, "Queued") || string.Equals(jobStatus.Status, "Running"))
            {
                await Task.Delay(1000);
                jobStatus = await ExecuteNewRequest<ExportJobStatusContract>(string.Format(GetUrl, jobId), Method.Get);
                if (sw.ElapsedMilliseconds > timeoutMilliSecond)
                    throw new Exception("Timeout period exceeded but export Job is still running.");
            }

            if (string.Equals(jobStatus.Status, "Success"))
                return jobStatus;
            else
                throw new Exception($"Export Job [{jobId}] status: {jobStatus.Status}");
        }

        public async Task DownloadExportedFile(ExportJobStatusContract jobStatus, string filePath)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (var request = NewRequest(jobStatus.File.EntityUri, Method.Get))
            {
                request.AddAuthorizationHeader(jobStatus.File.AuthorizationHeader);

                using (var response = await Send(request))
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    var responseStream = await response.ReadContentAsStreamAsync();
                    await responseStream.CopyToAsync(fs);
                }
            }
        }
    }
}
