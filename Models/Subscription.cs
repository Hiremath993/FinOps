using System.ComponentModel.DataAnnotations;

namespace Finops.Models
{
    public class Subscription
    {
        [Key]
        public string subscriptionId { get; set; }
        public string clientId { get; set; }
        public string tenantId { get; set; }
        public string clientSecret { get; set; }
    }
}
