using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SecByte.MockApi.Client
{
    public class CallDetails
    {
        private readonly string _path;
        private readonly JObject _body;

        public CallDetails(string path, JObject body)
        {
            _path = path;
            _body = body;
        }

        public string Path => _path;

        public JObject Body => _body;
    }
}
