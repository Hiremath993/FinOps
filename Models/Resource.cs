using Azure.ResourceManager.Resources.Models;
using Microsoft.Azure.Management.ResourceManager.Models;
using ServiceStack.Messaging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finops.Models
{
    public class Resources
    {
        [Key]
        public string Name { get; set; }
        public string ResourceId { get; set; }

        public string Type { get; set; }

        public string Location { get; set; }

        public int? TagId { get; set; }
        public Tag Tag { get; set; }
        //public string Owner { get; set; }

        // Other properties...

        //public int SubscriptionId { get; set; } 

        //public ICollection<Tag> Tags { get; set; }


        //public int TagId { get; set; }
        //[ForeignKey("Tag")]
        //public virtual Tag Tag { get; set; }
    }

    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        //public ICollection<Resources2> Resources2 { get; set; }
    }
}
