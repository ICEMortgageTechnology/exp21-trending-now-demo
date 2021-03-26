using System.Net.Http;

namespace IMT.DeveloperConnect.API.Client.Http
{
    public static class Method
    {
        public static HttpMethod Get => HttpMethod.Get;

        public static HttpMethod Put => HttpMethod.Put;

        public static HttpMethod Post => HttpMethod.Post;

        public static HttpMethod Delete => HttpMethod.Delete;

        public static HttpMethod Head => HttpMethod.Head;

        public static HttpMethod Options => HttpMethod.Options;

        public static HttpMethod Trace => HttpMethod.Trace;

        public static HttpMethod Patch { get; } = new HttpMethod("PATCH");
    }
}
