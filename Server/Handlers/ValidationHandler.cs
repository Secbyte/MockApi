using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal class ValidationHandler : IRequestHandler
    {
        public (int, string) ProcessRequest(string method, PathString path, string bodyText)
        {
            var requestMethod = new HttpMethod(path.GetSegment(1));
            var requestPath = string.Concat("/", path.FromSegment(2));
            var routeSetup = DataCache.RouteSetups.SingleOrDefault(r => r.Path == requestPath && r.Method == requestMethod);

            if (routeSetup != null)
            {
                var responseObject = new
                {
                    count = routeSetup.Requests.Count(),
                    requests = routeSetup.Requests.Select(rq => new
                    {
                        path = rq.path,
                        body = rq.request
                    })
                };

                return (200, Newtonsoft.Json.JsonConvert.SerializeObject(responseObject));
            }

            return (404, "Method not found");
        }
    }
}
