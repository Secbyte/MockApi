using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal interface IRequestHandler
    {
        (int Status, string Content) ProcessRequest(string method, PathString path, string bodyText);
    }
}
