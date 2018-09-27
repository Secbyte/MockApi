using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable CA1822 // Members that do not access instance data can be marked as static
namespace SecByte.MockApi.Server
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (System.IO.File.Exists("config/setup.json"))
            {
                RouteCache.LoadRoutes("config/setup.json");
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
    }
}
