using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SecByte.MockApi.Server
{
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(cb => cb.AddEnvironmentVariables())
                .UseStartup<Startup>();
        }
    }
}