using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SecByte.MockApi.Server.Handlers
{
    internal class WebRequestHandler : RequestHandler
    {        
        private readonly IFileReader _fileReader;

        public WebRequestHandler(RouteCache routeCache, IFileReader fileReader) : base(routeCache)
        {
            _fileReader = fileReader;
        }

        public override async Task<MockApiResponse> ProcessRequest(IHttpRequestFeature request)
        {            
            var requestMethod = new HttpMethod(request.Method);
            var requestPath = request.Path;
            var routeMatch = RouteCache.GetBestRouteMatch(requestMethod, requestPath);            

            if (routeMatch != null)
            {
                var bodyText = await request.Body?.ReadAsTextAsync();                
                routeMatch.Setup.LogRequest(request.Path, bodyText);
                var response = routeMatch.GetResponse(bodyText, request.GetQuery(), request.Headers);

                if(response.StartsWith("file:"))
                {
                    var templateFile = response.Split(":")[1];
                    try
                    {
                        response = await _fileReader.ReadContentsAsync(templateFile);
                    }
                    catch(System.IO.IOException)
                    {
                        return new MockApiResponse
                        {
                            StatusCode = 404,
                            Payload = $"Unable to load template {templateFile}",
                            ContentType = "text/plain"
                        };
                    }
                }

                return new MockApiResponse
                {
                    StatusCode = routeMatch.Setup.StatusCode,
                    Payload = response,
                    ContentType = "application/json; charset=utf-8"
                };
            }

            return new MockApiResponse
            {
                StatusCode = 404,
                Payload = "Method not set up",
                ContentType = "text/plain"
            };
        }
    }
}
