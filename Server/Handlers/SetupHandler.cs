using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal class SetupHandler : IRequestHandler
    {
        public async Task<MockApiResponse> ProcessRequest(HttpRequest request)
        {
            var path = request.Path;
            var method = request.GetMockApiMethod();
            var statusCode = request.GetMockApiStatus();
            var bodyAsText = await request.GetBodyAsText();

            DataCache.RouteSetups.RemoveAll(r => r.Path == path && r.Method == method);
            DataCache.RouteSetups.Add(new RouteSetup(method, path, bodyAsText, statusCode));

            return new MockApiResponse
            {
                StatusCode = 200,
                Payload = $"Setup path {method} {path}",
                ContentType = "text"
            };
        }
    }
}
