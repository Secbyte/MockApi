using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SecByte.MockApi.Server.Handlers
{
    internal class SetupHandler : RequestHandler
    {
        public SetupHandler(RouteCache routeCache) : base(routeCache)
        {            
        }

        public override async Task<MockApiResponse> ProcessRequest(IHttpRequestFeature request)
        {
            var path = request.Path;
            var method = request.GetMockApiMethod();
            var statusCode = request.GetMockApiStatus();
            var bodyAsText = await request.Body.ReadAsTextAsync();

            RouteCache.RegisterRouteSteup(method, path, bodyAsText, statusCode);

            return new MockApiResponse
            {
                StatusCode = 200,
                Payload = $"Setup path {method} {path}",
                ContentType = "text"
            };
        }
    }
}
