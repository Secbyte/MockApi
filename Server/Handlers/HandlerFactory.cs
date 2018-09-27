using System;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal static class HandlerFactory
    {
        public static IRequestHandler GetHandler(MockApiAction action)
        {
            switch (action)
            {
                case MockApiAction.Setup:
                    return new SetupHandler();
                case MockApiAction.Validate:
                    return new ValidationHandler();
                case MockApiAction.Call:
                    return new WebRequestHandler();
                case MockApiAction.BulkSetup:
                    return new BulkSetupHandler();
                default:
                    throw new NotSupportedException("Unsupported action type");
            }
        }
    }
}
