using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		public string GetResponse(string body)
		{
			var response = _routeSetup.Response;
			var placeholders = Regex.Matches(response, @"{([A-Za-z0-9\.\[\]]+)}");
			var jsonObject = BodyAsObject(body);

			foreach (Match placeholder in placeholders)
			{
				var key = placeholder.Groups[1].Value;
				if (_wildcards.ContainsKey(key))
				{
					response = response.Replace(placeholder.Value, _wildcards[key], StringComparison.InvariantCulture);
				}
				else
				{
					var valueFromBody = jsonObject.SelectToken(key);
					if (valueFromBody != null)
					{
						response = response.Replace(placeholder.Value, valueFromBody.ToString(), StringComparison.InvariantCulture);
					}
				}
			}

			return response;
		}       

		private static JObject BodyAsObject(string body)
		{
			if (string.IsNullOrEmpty(body))
			{
				return null;
			}

			return JObject.Parse(body);
		}
	}
}
