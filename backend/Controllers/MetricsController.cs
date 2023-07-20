using Finops.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

using System.Net.Http.Headers;

namespace Finops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Values2Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Values2Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(GetAuthorizationCode());
        }
        private static string GetAuthorizationCode()
        {
            string Cid = "5edb1685-bb33-44f5-acd8-c474c0c93695";
            string Csecret = "~eo8Q~QKXDuqu8V2Dkak2T8dEOwVySuoNXhFkcQz";
            string tid = "da3e59ae-984b-4580-991f-33a494213d4c";
            ClientCredential cc = new ClientCredential(Cid, Csecret);
            var context = new AuthenticationContext("https://login.microsoftonline.com/" + tid);
            var result = context.AcquireTokenAsync("https://management.azure.com/", cc);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to get access token");
            }
            return result.Result.AccessToken;
        }
        [HttpGet("GetResources")]
        public async Task<IActionResult> GetReources()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://management.azure.com/subscriptions/{_configuration.GetSection("SubscriptionId").Value}/resources?api-version=2021-04-01");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthorizationCode());

            // Send the request

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await MakeRequestAsync(request, client);

            return Ok(response);
        }
        [HttpGet("GetMetrics")]
        public async Task<IActionResult> GetMetrics()
        {
            string metricName = "Percentage CPU,Available Memory Bytes,Network In,Network Out";
            string output = null;
            string response = await GetAllResources();


            var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
            var resourceData = responseObject?.value;

            foreach (var item in resourceData)
            {
                Console.WriteLine(item.name);
                if (item.type == "Microsoft.Compute/virtualMachines")
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthorizationCode());
                    Console.WriteLine(item.name + " " + item.type);
                    string metricnp = item.type;
                    string metricnamespace = metricnp.ToLower();
                    client.BaseAddress = new Uri($"https://management.azure.com{item.id}/providers/Microsoft.Insights/metrics?api-version=2018-01-01&interval=P1D&timespan=P30D&metricnames={metricName}&aggregation=Average&metricnamespace={metricnamespace}");
                    HttpRequestMessage metricrequest = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

                    var metricresponse = await MakeRequestAsync(metricrequest, client);
                    if (output == null)
                    {

                        output = metricresponse;
                        continue;
                    }
                    output += "," + metricresponse;

                }

            }

            return Ok(output);
        }
        public static async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);

            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);

                return responseObject.ToString();
            }
            catch (HttpRequestException)
            {
                return "Error occur";
            }
        }
        public static async Task<string> GetAllResources()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthorizationCode());
            client.BaseAddress = new Uri($"https://management.azure.com/subscriptions/8309efe0-60f1-413a-90f0-ee27a0f0dbd2/resources?api-version=2021-04-01");


            // Send the request

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await MakeRequestAsync(request, client);


            return response.ToString();
        }
    }
}
