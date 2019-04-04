using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SecByte.MockApi.Server.Handlers
{
    internal class ValidationHandler : RequestHandler    
    {
        public ValidationHandler(RouteCache routeCache) : base(routeCache)
        {            
        }

        public override Task<MockApiResponse> ProcessRequest(IHttpRequestFeature request)
        {
            var requestMethod = request.GetMockApiMethod();
            var requestPath = request.Path;
            var routeSetup = RouteCache.GetRouteSteup(requestMethod, requestPath);

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

                return Task.FromResult(new MockApiResponse
                {
                    StatusCode = 200,
                    Payload = Newtonsoft.Json.JsonConvert.SerializeObject(responseObject),
                    ContentType = "application/json"
                });
            }

            return Task.FromResult(new MockApiResponse
            {
                StatusCode = 404,
                Payload = "Path not setup"
            });
        }
    }
}
