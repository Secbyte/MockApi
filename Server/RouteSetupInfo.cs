using Newtonsoft.Json.Linq;

namespace SecByte.MockApi.Server
{
    public class RouteSetupInfo
    {
        public string Path { get; set; }

        public int Status { get; set; }

        public string Method { get; set; }

        public object Response { get; set; }
    }
}