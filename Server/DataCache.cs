using System.Collections.Generic;

namespace SecByte.MockApi.Server
{
    public static class DataCache
    {
        private static List<RouteSetup> _routeSetups = new List<RouteSetup>();

        public static List<RouteSetup> RouteSetups => _routeSetups;
    }
}