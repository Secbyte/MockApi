using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SecByte.MockApi.Server
{
    public class RouteSetup
    {
        private readonly HttpMethod _method;
        private readonly string _path;
        private readonly string _response;
        private readonly int _status;
        private readonly List<(string path, string request)> _requests;

        public RouteSetup(HttpMethod method, string path, string response, int status)
        {
            _method = method;
            _path = path;
            _response = response;
            _status = status;
            _requests = new List<(string, string)>();
        }

        public HttpMethod Method => _method;

        public string Path => _path;

        public string Response => _response;

        public int StatusCode => _status;

        public IEnumerable<(string path, string request)> Requests => _requests.ToList().AsReadOnly();

        public void LogRequest(string path, string request)
        {
            _requests.Add((path, request));
        }

        public RouteMatch MatchesOn(HttpMethod method, string requestPath)
        {
            Console.WriteLine(method + " = " + _method);

            if (method == _method)
            {
                var routeParts = _path.Split('/');
                var requestParts = requestPath.Split('/');
                var wildcards = new Dictionary<string, string>();

                if (routeParts.Length == requestParts.Length)
                {
                    for (int i = 0; i < routeParts.Length; i++)
                    {
                        var routePart = routeParts[i];
                        var requestPart = requestParts[i];

                        if (routePart.StartsWith('{'))
                        {
                            var wildcardKey = routePart.Substring(1, routePart.Length - 2);
                            wildcards.Add(wildcardKey, requestPart);
                        }
                        else if (routePart != requestPart)
                        {
                            return RouteMatch.NoMatch;
                        }
                    }

                    return new RouteMatch(this, wildcards);
                }
            }

            return RouteMatch.NoMatch;
        }
    }
}
