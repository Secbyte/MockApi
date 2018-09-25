using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server
{
    public static class HttpRequestExtensions
    {
        public static MockApiAction GetMockApiAction(this HttpRequest request)
        {
            Func<string, MockApiAction> parseAction = (s) => (MockApiAction)Enum.Parse(typeof(MockApiAction), s);
            const string header = "MockApi-Action";
            return request.Headers.ContainsKey(header) ? parseAction(request.Headers[header]) : MockApiAction.Call;
        }

        public static HttpMethod GetMockApiMethod(this HttpRequest request)
        {
            const string header = "MockApi-Method";
            if (request.Headers.ContainsKey(header))
            {
                return new HttpMethod(request.Headers[header]);
            }

            return HttpMethod.Get;
        }

        public static int GetMockApiStatus(this HttpRequest request)
        {
            const string header = "MockApi-Status";
            if (request.Headers.ContainsKey(header))
            {
                return int.Parse(request.Headers[header], null);
            }

            return 200;
        }

        public static async Task<string> GetBodyAsText(this HttpRequest request)
        {
            if (request.ContentLength.GetValueOrDefault() > 0)
            {
                var buffer = new byte[(int)request.ContentLength];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }

            return string.Empty;
        }
    }
}