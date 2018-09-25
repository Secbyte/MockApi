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

            app.Run(async (context) =>
            {
                var path = context.Request.Path;
                var bodyAsText = string.Empty;

                if (context.Request.ContentLength.GetValueOrDefault() > 0)
                {
                    var buffer = new byte[(int)context.Request.ContentLength];
                    await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                    bodyAsText = Encoding.UTF8.GetString(buffer);
                }

                var handler = Handlers.HandlerFactory.GetHandler(path);
                var response = handler.ProcessRequest(context.Request.Method, path, bodyAsText);
                context.Response.StatusCode = response.Status;
                context.Response.Headers.Add("content-type", "application/json; charset=utf-8");
                context.Response.Headers.Add("access-control-allow-origin", "*");
                await context.Response.WriteAsync(response.Content);
            });
        }
    }
}
