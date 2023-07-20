using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Management.ResourceManager;

using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Microsoft.Rest;

using Finops.Data;

using Finops.Models;

namespace ResourcesWebApi.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class SubscriptionController : ControllerBase

    {

        private readonly FinopsDbContext resourceDbContext;




        public SubscriptionController(FinopsDbContext resourceDbContext)

        {

            this.resourceDbContext = resourceDbContext;

        }




        [HttpGet]
        public async Task<IActionResult> GetAllResources([FromQuery] Subscription data)

        {

            var tenantId = data.tenantId;

            var clientId = data.clientId;

            // var objectId = data.objectId;

            var clientSecret = data.clientSecret;

            var subscriptionId = data.subscriptionId;




            var credentials = new ClientCredential(clientId, clientSecret);

            var context = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}");

            var result = await context.AcquireTokenAsync("https://management.azure.com/", credentials);

            var accessToken = result.AccessToken;




            var resourceClient = new ResourceManagementClient(new TokenCredentials(accessToken));

            resourceClient.SubscriptionId = subscriptionId;




            var resources = await resourceClient.Resources.ListAsync();




            var subscriptionExists = resources.Any();




            if (subscriptionExists)

            {
                var resourcesData = new Subscription

                {

                    clientId = data.clientId,

                    tenantId = data.tenantId,

                    subscriptionId = data.subscriptionId,

                    clientSecret = data.clientSecret

                };




                resourceDbContext.Subscription.Add(resourcesData);

                await resourceDbContext.SaveChangesAsync();




                return Ok(new { SubscriptionExists = true });

            }

            else

            {

                return Ok("Invalid Subscription");

            }

        }




        // var response = new { SubscriptionExists = subscriptionExists };





        //     foreach (var resource in resources)

        //     {

        //         Console.WriteLine($"Id: {resource.Id}");




        //         if (resource.Id != null)

        //         {

        //        //  Console.WriteLine("Subscription exists:");

        //          return Ok(response);

        //         }

        //         else

        //         {

        //           // Console.WriteLine("Invalid Subscription:");

        //             return Ok("Invalid Subscription:");

        //         }




        //         if (resource.Tags != null)

        //         {

        //             foreach (var tag in resource.Tags)

        //             {

        //                 Console.WriteLine($"{tag.Key}: {tag.Value}");

        //             }

        //         }






        //        Console.WriteLine();

        //     }




        //     return Ok(resources);

        // }




        // [HttpPost]

        // public async Task<IActionResult> AddResource(Resources resource)

        // {

        //     resourceDbContext.Resources.Add(resource);

        //     await resourceDbContext.SaveChangesAsync();




        //     if (resource.Tags != null)

        //     {

        //         foreach (var tag in resource.Tags)

        //         {

        //             var Tag = new Tag

        //             {

        //                 Key = tag.Key,

        //                 Value = tag.Value,

        //                 ResourceId = resource.Id

        //             };




        //             resourceDbContext.Tags.Add(Tag);

        //         }

        //         await resourceDbContext.SaveChangesAsync();

        //     }




        //     return Ok(resource);

        // }

    }

}