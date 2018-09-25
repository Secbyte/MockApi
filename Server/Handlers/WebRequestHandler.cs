using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    public class WebRequestHandler : IRequestHandler
    {
        public async Task<MockApiResponse> ProcessRequest(HttpRequest request)
        {
            var routeMatch = DataCache.RouteSetups
                            .Select(r => r.MatchesOn( new HttpMethod(request.Method), request.Path))
                            .OrderBy(r => r.WildcardCount)
                            .FirstOrDefault(rm => rm.Success);

            if (routeMatch != null)
            {
                var bodyText = await request.GetBodyAsText();
                routeMatch.Setup.LogRequest(request.Path, bodyText);
                return new MockApiResponse
                {
                    StatusCode = routeMatch.Setup.StatusCode,
                    Payload = routeMatch.GetResponse(bodyText),
                    ContentType = "application/json; charset=utf-8"
                };
            }

            return new MockApiResponse
            {
                StatusCode = 404,
                Payload = "Method not set up"
            };
        }
    }
}
