using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server
{
    internal static class RouteCache
    {
        private static List<RouteSetup> _routeSetups = new List<RouteSetup>();

        public static void LoadRoutes(string setupFile)
        {
            var routes = Newtonsoft.Json.JsonConvert.DeserializeObject<RouteSetupInfo[]>(System.IO.File.ReadAllText(setupFile));
            foreach (var route in routes)
            {
                var method = new HttpMethod(route.Method);
                var response = route.Response.ToString();
                RegisterRouteSteup(method, route.Path, response, route.Status);
            }
        }

        public static void RegisterRouteSteup(HttpMethod method, PathString path, string response, int statusCode)
        {
            _routeSetups.RemoveAll(r => r.Path == path && r.Method == method);
            _routeSetups.Add(new RouteSetup(method, path, response, statusCode));
        }

        public static RouteSetup GetRouteSteup(HttpMethod method, PathString path)
        {
            return _routeSetups.SingleOrDefault(r => r.Path == path && r.Method == method);
        }

        public static RouteMatch GetBestRouteMatch(HttpMethod method, PathString path)
        {
            return _routeSetups.Select(r => r.MatchesOn(method, path))
                    .OrderBy(r => r.WildcardCount)
                    .FirstOrDefault(rm => rm.Success);
        }
    }
}