using Finops.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finops.Data;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

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

            foreach (var resource in resources)
            {
                //var dbad = finopsDbContext.Resources.FindAsync(resource.Name);
                //if (dbad != null) continue;
                //return Ok(dbad);
                //Console.WriteLine(dbad);
                Console.WriteLine(resource.Name);
                //return Ok(resource);
                //var resourcedata= JsonConvert.DeserializeObject<Dictionary<string, object>>(resource);
                try { 
                    var no = resource.Tags;
                    int? TagId = null;
                    if (no == null)
                    {
                        var rees = new Resources
                        {
                            Name = resource.Name,
                            ResourceId = resource.Id,
                            Type = resource.Type,
                            Location = resource.Location,
                            TagId = TagId
                        };
                        finopsDbContext.Resources.AddRange(rees);
                        await finopsDbContext.SaveChangesAsync();
                        continue;
                    }
                    foreach (var tag in no)
                    {
                        //CHECKING
                        //var c = finopsDbContext.Resources2.FindAsync(1);
                        //return Ok(c);
                        //
                        var res = new Tag
                        {
                            Key = tag.Key.ToString(),
                            Value = tag.Value.ToString()
                        };

                        finopsDbContext.Tag.AddRange(res);
                        await finopsDbContext.SaveChangesAsync();

                        var rees = new Resources
                        {
                            ResourceId = resource.Id,
                            Name = resource.Name,
                            Type = resource.Type,
                            Location = resource.Location,
                            TagId = res.TagId
                        };
                        finopsDbContext.Resources.AddRange(rees);
                        await finopsDbContext.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Looks like resource name already exist in DB as primary key");
                    continue;
                }
            }
            return Ok(await finopsDbContext.Resources.ToListAsync());
        }
    }
}