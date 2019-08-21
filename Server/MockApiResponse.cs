using System.Collections.Generic;

namespace SecByte.MockApi.Server
{
    public class MockApiResponse
    {
        public string Payload { get; set; }

        public int StatusCode { get; set; }

        public string ContentType { get; set; }

        public Dictionary<string, string> Headers {get; set; }

        public MockApiResponse()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}