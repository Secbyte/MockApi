using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SecByte.MockApi.Server.Handlers
{
    internal abstract class RequestHandler
    {
        protected RequestHandler(RouteCache routeCache)
        {
            RouteCache = routeCache;
        }

        public RouteCache RouteCache { get; }

        public abstract Task<MockApiResponse> ProcessRequest(IHttpRequestFeature request);
    }
}
