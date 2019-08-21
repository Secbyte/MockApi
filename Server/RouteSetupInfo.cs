using System.Collections.Generic;

namespace SecByte.MockApi.Server
{
    public class RouteSetupInfo
    {
        public string Path { get; set; }

        public int Status { get; set; }

        public string Method { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public object Response { get; set; }

        public RouteSetupInfo() 
        {
            Headers = new Dictionary<string, string>();
        }
    }
}