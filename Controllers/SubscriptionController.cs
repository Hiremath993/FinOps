using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Finops.Data;
using Finops.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var credentials = new ClientCredential(data.clientId, data.clientSecret);
                var context = new AuthenticationContext($"https://login.microsoftonline.com/{data.tenantId}");
                var result = await context.AcquireTokenAsync("https://management.azure.com/", credentials);
                bool accessTokenbool = result.AccessToken.Any();                
                var accessToken = result.AccessToken;

                var resourceClient = new ResourceManagementClient(new TokenCredentials(accessToken));
                resourceClient.SubscriptionId = data.subscriptionId;

                var resources = await resourceClient.Resources.ListAsync();
                //checking Subscription Really exist in Azure or not
                var subscriptionExists = resources.Any();
                //checking SubscriptionId already present in database or not
                var subscriptiontab = await resourceDbContext.Subscription.FindAsync(data.subscriptionId);
                
                //return Ok(subscriptionExists);
                if (subscriptiontab!=null)
                {
                    return BadRequest("Subscription Id already Exist in Database");
                }
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
                    return BadRequest("Invalid Subscription");
                }
            }
            catch (Exception)
            {
                return BadRequest("Invalid Inputs");
            }
            

            
        }
    }
}