using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace SecByte.MockApi.Server
{
    internal class RouteCache
    {
        private readonly List<RouteSetup> _routeSetups = new List<RouteSetup>();
        private readonly IFileReader _fileReader;
        private readonly Options _options;

        public RouteCache(IFileReader fileReader, IOptions<Options> options)
        {
            _fileReader = fileReader;
            _options = options.Value;
        }

        public async Task Initialise()
        {
            if (string.IsNullOrEmpty(_options.RoutesFile) == false && _routeSetups.Any() == false)
            {
                System.Console.WriteLine(_options.RoutesFile);
                LoadRoutes(await _fileReader.ReadContentsAsync(_options.RoutesFile));
            }
        }

        public void LoadRoutes(string routesDocument)
        {
            var routes = Newtonsoft.Json.JsonConvert.DeserializeObject<RouteSetupInfo[]>(routesDocument);
            RegisterRoutes(routes);
        }

        public void RegisterRouteSteup(HttpMethod method, PathString path, string response, int statusCode)
        {
            RegisterRouteSteup(method, path, response, statusCode);
        }

        public void RegisterRouteSteup(HttpMethod method, PathString path, string response, int statusCode, Dictionary<string, string> headers)
        {
            _routeSetups.RemoveAll(r => r.Path == path && r.Method == method);
            _routeSetups.Add(new RouteSetup(method, path, response, statusCode, headers));
        }

        public RouteSetup GetRouteSteup(HttpMethod method, PathString path)
        {
            return _routeSetups.SingleOrDefault(r => r.Path == path && r.Method == method);
        }

        public RouteMatch GetBestRouteMatch(HttpMethod method, PathString path)
        {
            return _routeSetups.Select(r => r.MatchesOn(method, path))
                    .OrderBy(r => r.WildcardCount)
                    .FirstOrDefault(rm => rm.Success);
        }

        private void RegisterRoutes(RouteSetupInfo[] routes)
        {
            foreach (var route in routes)
            {
                var method = new HttpMethod(route.Method);
                var response = route.Response.ToString();                
                RegisterRouteSteup(method, route.Path, response, route.Status, route.Headers);
            }
        }

        public class Options
        {
            public string RoutesFile { get; set; }
        }
    }
}