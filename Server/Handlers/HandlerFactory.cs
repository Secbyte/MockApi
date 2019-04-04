using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SecByte.MockApi.Server.Handlers
{
    internal class HandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public RequestHandler GetHandler(MockApiAction action)
        {
            switch (action)
            {
                case MockApiAction.Setup:
                    return _serviceProvider.GetService<SetupHandler>();
                case MockApiAction.Validate:
                    return _serviceProvider.GetService<ValidationHandler>();
                case MockApiAction.Call:
                    return _serviceProvider.GetService<WebRequestHandler>();
                case MockApiAction.BulkSetup:
                    return _serviceProvider.GetService<BulkSetupHandler>();
                default:
                    throw new NotSupportedException("Unsupported action type");
            }
        }
    }
}
