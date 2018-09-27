using System;
using SecByte.MockApi.Client;

namespace Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mockApiClient = new MockApiClient("http://localhost:5678");
            mockApiClient.SetupFromFile(@"c:\dockershare\mockapi\config\setup.json").GetAwaiter().GetResult();
            Console.WriteLine("Done");
        }
    }
}
