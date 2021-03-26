using System.Net.Http;

namespace IMT.DeveloperConnect.API.Client.Http.Content
{
    public class FileContentBuilder : IContentBuilder
    {
        private readonly string _filePath;
        private readonly string _contentType;

        public FileContentBuilder(string filePath, string contentType)
        {
            _filePath = filePath;
            _contentType = contentType;
        }

        public HttpContent Build()
        {
            return new FileContent(_filePath, _contentType);
        }

        public string GetPayloadForLogging()
        {
            return $"[File: {_filePath}]";
        }
    }
}
