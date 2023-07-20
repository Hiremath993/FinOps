using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Finops.Models
{
    public class Resources
    {
        [Key]
        public int Id { get; set; }
        public string ResourceId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Location { get; set; }

        
        // Other properties...

        //public int SubscriptionId { get; set; } 

        public ICollection<Tag> Tags { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ResourceId { get; set; }
    }
}
