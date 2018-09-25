using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server
{
    public static class PathStringExtensions
    {
        public static string[] GetSegments(this PathString pathString)
        {
            return pathString.Value.Split("/", StringSplitOptions.RemoveEmptyEntries);            
        }       
    }
}