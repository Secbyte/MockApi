using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace SecByte.MockApi.Server.Handlers
{
    public class WebRequestHandler : IRequestHandler
    {
        public async Task<MockApiResponse> ProcessRequest(HttpRequest request)
        {
            var requestMethod = request.GetMockApiMethod();
            var requestPath = request.Path;
            var routeMatch = RouteCache.GetBestRouteMatch(requestMethod, requestPath);

            if (routeMatch != null)
            {
                var bodyText = await request.GetBodyAsText();
                routeMatch.Setup.LogRequest(request.Path, bodyText);
                var response = JToken.Parse(routeMatch.GetResponse(bodyText));                                

                if(response is JArray items)
                {
                    var stuff = new JArray();                    

                    foreach(var item in items.Cast<JObject>())
                    {
                        if(item.ContainsKey("__Template"))
                        {
                            var templateItem = item.ToObject<TemplateItem>();
                            for(int i = 0; i < templateItem.Count; i++)
                            {
                                stuff.Add( JToken.Parse("{ \"Message\": \"Hello from template " + templateItem.__Template + "\" }") );
                            }                            
                        }
                        else
                            stuff.Add(item);
                    }

                    return new MockApiResponse
                    {
                        StatusCode = routeMatch.Setup.StatusCode,
                        Payload = stuff.ToString(),
                        ContentType = "application/json; charset=utf-8"
                    };
                }
                else
                {
                    return new MockApiResponse
                    {
                        StatusCode = routeMatch.Setup.StatusCode,
                        Payload = response.ToString(),
                        ContentType = "application/json; charset=utf-8"
                    };
                }
            }

            return new MockApiResponse
            {
                StatusCode = 404,
                Payload = "Method not set up"
            };
        }
    }

    public class TemplateItem
    {
        public string __Template { get; set; }
        public int Count { get; set; } 
    }
}
