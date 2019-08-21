using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecByte.MockApi.Server.Handlers;

#pragma warning disable CA1822 // Members that do not access instance data can be marked as static
namespace SecByte.MockApi.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<BulkSetupHandler>()
                .AddScoped<HandlerFactory>()
                .AddScoped<SetupHandler>()
                .AddScoped<ValidationHandler>()
                .AddScoped<WebRequestHandler>();       

            var dataSource = Configuration.GetValue<string>("DataSource");
            if (string.IsNullOrEmpty(dataSource) == false)
            {
                var dataSourceParts = dataSource.Split(":");
                services.Configure<FileReaderOptions>(opt => opt.Root = dataSourceParts[1]);
                switch (dataSourceParts[0])
                {
                    case "local":
                        services.AddSingleton<IFileReader, FileSystemFileReader>();
                        break;
                    case "s3":
                        services.AddAWSService<IAmazonS3>();                        
                        services.AddSingleton<IFileReader, S3FileReader>();
                        break;
                    default:
                        throw new NotSupportedException($"Data source {dataSourceParts[0]} is not supported");
                }                
            }

            var routesFile = Configuration.GetValue<string>("RoutesFile");
            services
                .Configure<RouteCache.Options>(opt => opt.RoutesFile = routesFile)
                .AddSingleton<RouteCache>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var routeCache = context.RequestServices.GetRequiredService<RouteCache>();
                await routeCache.Initialise();
                                
                var handlerFactory = context.RequestServices.GetRequiredService<HandlerFactory>();
                var requestInfo = context.Features.Get<IHttpRequestFeature>();
                var handler = handlerFactory.GetHandler(requestInfo.GetMockApiAction());
                var response = await handler.ProcessRequest(requestInfo);
                context.Response.StatusCode = response.StatusCode;
                context.Response.Headers.Add("content-type", response.ContentType);
                
                foreach(var header in response.Headers)                
                    context.Response.Headers.Add(header.Key, header.Value);                

                await context.Response.WriteAsync(response.Payload);
            });
        }        
    }
}
