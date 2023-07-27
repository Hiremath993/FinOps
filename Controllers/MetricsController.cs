using Finops.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Finops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MetricsController(IConfiguration configuration)
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
        public async Task<IActionResult> GetResources()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://management.azure.com/subscriptions/8309efe0-60f1-413a-90f0-ee27a0f0dbd2/resources?api-version=2021-04-01");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthorizationCode());

            // Send the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            var response = await MakeRequestAsync(request, client);



            //foreach (var resp in response)
            //{
            //    var res = new Resources
            //    {
            //        ResourceId = resp.Id,
            //        Name = resp.Name,
            //        Type = resp.Type,
            //        Location = resp.Location,
            //    };
            //}

            return Ok(response);
        }

        [HttpGet("GetMetrics")]
        public async Task<IActionResult> GetMetrics()
        {
            string metricName = "Percentage CPU,Available Memory Bytes,Network In,Network Out";
            string output = null;
            string response = await GetAllResources();

            var responseObject = JsonConvert.DeserializeObject<dynamic>(response);
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
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                return responseObject.ToString();
            }
            catch (HttpRequestException)
            {
                return "Error occur";
            }
        }
        //using in metric api
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
        public static async Task<string> GetVMCreationDate(string id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthorizationCode());
            client.BaseAddress = new Uri($"https://management.azure.com{id}?$expand=userData&api-version=2023-03-01");

            // Send the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            var response = await MakeRequestAsync(request, client);
            var responseObject = JsonConvert.DeserializeObject<dynamic>(response);
            var metriccpuData = responseObject?.properties?.timeCreated;

            return metriccpuData;
        }

        [HttpGet("CalculateAverageMetrics")]
        public async Task<IActionResult> CalculateAverageMetrics()
        {
            string metricName = "Percentage CPU,Available Memory Bytes,Network In,Network Out";
            string response = await GetAllResources();

            var responseObject = JsonConvert.DeserializeObject<dynamic>(response);
            var resourceData = responseObject?.value;

            string output = null;
            foreach (var item in resourceData)
            {
                if (item.type == "Microsoft.Compute/virtualMachines")
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    string Creationdate = await GetVMCreationDate(item.id.ToString());
                    DateTime creationdate = Convert.ToDateTime(Creationdate, culture);
                    var ma = new MetricAverage
                    {
                        Id = item.id,
                        Name = item.name,
                        Namespace = item.type,
                        Location = item.location,
                        TimeCreated = creationdate,
                    };
                    ma.Average = new Dictionary<string, double>();
                    Console.WriteLine("Resource: " + item.name + "--------------------------------------------------------------");
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthorizationCode());
                    string metricnp = item.type;
                    string metricnamespace = metricnp.ToLower();
                    client.BaseAddress = new Uri($"https://management.azure.com{item.id}/providers/Microsoft.Insights/metrics?api-version=2018-01-01&interval=P1D&timespan=P30D&metricnames={metricName}&aggregation=Average&metricnamespace={metricnamespace}");
                    HttpRequestMessage metricrequest = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

                    var metricresponse = await MakeRequestAsync(metricrequest, client);

                    var metricObject = JsonConvert.DeserializeObject<dynamic>(metricresponse);
                    var metricValues = metricObject?.value;

                    if (metricValues != null && metricValues.HasValues)
                    {
                        foreach (var metricValue in metricValues)
                        {
                            int count = 0;
                            double sum = 0;
                            string metricNameValue = metricValue?.name?.value?.ToString();
                            Console.WriteLine("Metrics: " + metricNameValue + "-------------------------------");


                            var metricTimeSeries = metricValue?.timeseries;
                            foreach (var v in metricTimeSeries[0]?.data)
                            {
                                DateTime md = v?.timeStamp;
                                Console.WriteLine("CreationDate: " + creationdate);
                                Console.Write("TimeStamp: " + md);
                                if (creationdate.Date <= md.Date)
                                {
                                    Console.WriteLine("Greater than the creation datetime!");
                                    count += 1;
                                    if (v["average"] != null && v["average"].Type != JTokenType.Null)
                                    {
                                        sum += v.Value<float>("average");
                                        Console.WriteLine("Average: " + v["average"]);
                                    }
                                    Console.WriteLine("Count: " + count);
                                    Console.WriteLine("Sum: " + sum);
                                }
                                else
                                {
                                    Console.WriteLine("Average: " + v["average"]);
                                    Console.WriteLine("less");
                                }
                            }
                            if (count == 0)
                            {
                                ma.Average.Add(metricNameValue, 0);
                                continue;
                            }
                            ma.Average.Add(metricNameValue, (double)(sum / count));
                        }
                    }
                    var json = JsonConvert.SerializeObject(ma, Formatting.Indented);
                    if (output == null)
                    {
                        output = json;
                        continue;
                    }
                    output += "," + json;
                }
            }
            return Ok(output);
        }
    }
}





