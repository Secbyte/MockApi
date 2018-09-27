using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace SecByte.MockApi.Client
{
    public class MockApiClient
    {
        private readonly string _mockApiHost;

        public MockApiClient(string mockApiHost)
        {
            _mockApiHost = mockApiHost;
        }

        public async Task SetupFromFile(string filePath)
        {
            if(!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException(filePath);
            }

            await SetupFromJson(File.ReadAllText(filePath));
        }

        public async Task SetupFromJson(string routesDocument)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("MockApi-Action", "BulkSetup");
                var response = await client.PostAsync(_mockApiHost, new StringContent(routesDocument, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
        }

        public MockApiAction Setup(string method, string path)
        {
            return new MockApiAction(_mockApiHost, method, path);
        }

        public async Task<IEnumerable<CallDetails>> Calls(string method, string path)
        {
            var uri = $"{_mockApiHost}/{path}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("MockApi-Action", "Validate");
                client.DefaultRequestHeaders.Add("MockApi-Method", method);

                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var bodyAsString = await response.Content.ReadAsStringAsync();
                var bodyAsJson = JObject.Parse(bodyAsString);

                var requests = bodyAsJson.SelectToken("requests") as JArray;
                var results = new List<CallDetails>();

                foreach (var request in requests.Children())
                {
                    JObject body = null;
                    var temp = request["body"].ToString();
                    if (string.IsNullOrEmpty(temp))
                    {
                        body = JObject.Parse(temp);
                    }

                    results.Add(new CallDetails(request["path"].ToString(), body));
                }

                return results;
            }
        }
    }
}