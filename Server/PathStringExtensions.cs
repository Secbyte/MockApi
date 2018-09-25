using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server
{
    public static class PathStringExtensions
    {
        public static string GetSegment(this PathString pathString, int segmentIndex)
        {
            var segments = pathString.Value.Split("/", StringSplitOptions.RemoveEmptyEntries);
            return segments[segmentIndex];
        }

        public static string FromSegment(this PathString pathString, int segmentIndex)
        {
            var segments = pathString.Value.Split("/", StringSplitOptions.RemoveEmptyEntries).Skip(segmentIndex);
            return string.Join("/", segments);
        }
    }
}