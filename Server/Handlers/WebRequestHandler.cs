using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    public class WebRequestHandler : IRequestHandler
    {
        public (int, string) ProcessRequest(string method, PathString path, string bodyText)
        {
            var requestMethod = new HttpMethod(method);
            var routeMatch = DataCache.RouteSetups
                            .Select(r => r.MatchesOn(requestMethod, path))
                            .OrderBy(r => r.WildcardCount)
                            .FirstOrDefault(rm => rm.Success);

            if (routeMatch != null)
            {
                routeMatch.Setup.LogRequest(path, bodyText);
                return (routeMatch.Setup.StatusCode, routeMatch.GetResponse(bodyText));
            }

            return (404, string.Empty);
        }
    }
}
