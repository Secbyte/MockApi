using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal class BulkSetupHandler : IRequestHandler
    {
        public async Task<MockApiResponse> ProcessRequest(HttpRequest request)
        {
            var routesDocument = await request.GetBodyAsText();

            RouteCache.LoadRoutesFromJson(routesDocument);

            return new MockApiResponse
            {
                StatusCode = 200,
                Payload = $"Bulk setup complete",
                ContentType = "text"
            };
        }
    }
}
