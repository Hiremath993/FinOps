using Finops.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finops.Data;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceManager.Models;

namespace Finops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly FinopsDbContext finopsDbContext;

        public ResourceController(FinopsDbContext finopsDbContext)
        {
            this.finopsDbContext = finopsDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResources()
        {

            var tenantId = "da3e59ae-984b-4580-991f-33a494213d4c";
            var clientId = "5edb1685-bb33-44f5-acd8-c474c0c93695";
            var clientSecret = "~eo8Q~QKXDuqu8V2Dkak2T8dEOwVySuoNXhFkcQz";
            var subscriptionId = "8309efe0-60f1-413a-90f0-ee27a0f0dbd2";

            var credentials = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}");
            var result = await context.AcquireTokenAsync("https://management.azure.com/", credentials);
            var accessToken = result.AccessToken;
            var resourceClient = new ResourceManagementClient(new TokenCredentials(accessToken));
            resourceClient.SubscriptionId = subscriptionId;
            
            var resources = await resourceClient.Resources.ListAsync();
            var resourceList = new List<Resources>();

            foreach (var resource in resources)
            {
               var res = new Resources
                {
                    ResourceId = resource.Id,
                    Name = resource.Name,
                    Type = resource.Type,
                    Location = resource.Location,
                };

                if (resource.Tags != null)
                { 
                    res.Tags = resource.Tags.Select(t => new Tag
                    { 
                        Key = t.Key,

                        Value = t.Value

                    }).ToList();

                }
                resourceList.Add(res);

            }

            // Add the fetched resources to the database and save changes

            finopsDbContext.Resources.AddRange(resourceList);
            await finopsDbContext.SaveChangesAsync();

            return Ok(resourceList);

        }
    }

}