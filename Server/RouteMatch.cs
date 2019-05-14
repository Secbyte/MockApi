using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace SecByte.MockApi.Server
{
    public class RouteMatch
    {
        private readonly RouteSetup _routeSetup;
        private readonly bool _success;
        private readonly Dictionary<string, string> _wildcards;

        public RouteMatch(RouteSetup routeSetup, Dictionary<string, string> wildcards)
        {
            _success = true;
            _wildcards = wildcards;
            _routeSetup = routeSetup;
        }

        private RouteMatch(bool success)
        {
            _success = success;
        }

        public static RouteMatch NoMatch => new RouteMatch(false);

        public bool Success => _success;

        public RouteSetup Setup => _routeSetup;

        public int WildcardCount => _wildcards.Count;

        public string GetResponse(string body, Dictionary<string, StringValues> query, IHeaderDictionary headers)
        {
            var response = _routeSetup.Response;
            var placeholders = Regex.Matches(response, @"{([A-Za-z0-9\.\[\]]+)}");
            var payloadObjects = BodyAsObject(body);

            foreach (Match placeholder in placeholders)
            {
                var key = placeholder.Groups[1].Value;
                if (_wildcards.ContainsKey(key))
                {
                    response = response.Replace(placeholder.Value, _wildcards[key], StringComparison.InvariantCulture);
                }
                else if(query.ContainsKey(key))
                {
                    response = response.Replace(placeholder.Value, query[key].First(), StringComparison.InvariantCulture);
                }                
                else if(payloadObjects.Any())
                {
                    foreach(var obj in payloadObjects)
                    {
                        var valueFromBody = obj.SelectToken(key);
                        if (valueFromBody != null)
                        {
                            response = response.Replace(placeholder.Value, valueFromBody.ToString(), StringComparison.InvariantCulture);
                            break;
                        }
                    }
                }
                else if(headers.ContainsKey(key))
                {
                    response = response.Replace(placeholder.Value, headers[key].First(), StringComparison.InvariantCulture);
                }
            }

            return response;
        }

        private static JArray BodyAsObject(string body)
        {
            if (body.StartsWith('[') == false)
                body = $"[{body}]";

            return JArray.Parse(body);
        }
    }
}
