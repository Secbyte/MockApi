using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal class SetupHandler : IRequestHandler
    {
        public (int, string) ProcessRequest(string method, PathString path, string bodyText)
        {
            var targetMethod = new HttpMethod(path.GetSegment(1));
            var statusCode = 200;
            var requestPath = string.Concat("/", path.FromSegment(2));
            var possibleStatusSegment = path.GetSegment(2);

            if (Regex.IsMatch(possibleStatusSegment, @"[1-5]\d{2}") )
            {
                statusCode = int.Parse(possibleStatusSegment, null);
                requestPath = "/" + path.FromSegment(3);
            }

            Console.WriteLine($"{path} - {targetMethod} - {requestPath}");

            DataCache.RouteSetups.RemoveAll(r => r.Path == requestPath && r.Method == targetMethod);
            DataCache.RouteSetups.Add(new RouteSetup(targetMethod, requestPath, bodyText, statusCode));
            return (200, path);
        }
    }
}
