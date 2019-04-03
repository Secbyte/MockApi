using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var configSource = Configuration.GetValue<string>("ConfigurationSource");

            if (string.IsNullOrEmpty(configSource) == false)
            {
                var configData = GetConfigData(configSource).GetAwaiter().GetResult();
                RouteCache.LoadRoutes(configData);
            }

            app.Run(async (context) =>
            {
                var handler = Handlers.HandlerFactory.GetHandler(context.Request.GetMockApiAction());
                var response = await handler.ProcessRequest(context.Request);
                context.Response.StatusCode = response.StatusCode;
                context.Response.Headers.Add("content-type", response.ContentType);
                context.Response.Headers.Add("access-control-allow-origin", "*");
                await context.Response.WriteAsync(response.Payload);
            });
        }

        public async Task<string> GetConfigData(string configSource)
        {
            var configParts = configSource.Split(":");
            switch (configParts[0])
            {
                case "file":
                    return await System.IO.File.ReadAllTextAsync(configParts[1]);
                case "s3":
                    return await ReadFromS3(configParts[1], configParts[2]);
                default:
                    throw new NotSupportedException("config source not supported");
            }
        }

        public async Task<string> ReadFromS3(string bucketName, string key)
        {
            var aws = Configuration.GetAWSOptions();
            var s3Service = aws.CreateServiceClient<IAmazonS3>();

            var s3Response = await s3Service.GetObjectAsync(bucketName, key);
            if ((int)s3Response.HttpStatusCode != 200)
            {
                throw new FileLoadException($"Unable to retrieve the configuration document from {bucketName}:{key}");
            }

            using (var streamReader = new StreamReader(s3Response.ResponseStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
