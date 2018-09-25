using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecByte.MockApi.Server.Handlers
{
    internal interface IRequestHandler
    {
        Task<MockApiResponse> ProcessRequest(HttpRequest request);
    }
}
