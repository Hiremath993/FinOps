using Finops.Models;
using Microsoft.EntityFrameworkCore;
using resource.Models;

namespace Finops.Data
{
    public class FinopsDbContext:DbContext
    {
        public FinopsDbContext(DbContextOptions options) : base (options)
            {       
        }

        public DbSet<Login> Login { get; set; }
        public DbSet<Subscription> Subscription { get; set; }

        public DbSet<Resources> Resources { get; set; }

        public DbSet<Tag> Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the foreign key relationship
            modelBuilder.Entity<Resources>()
                .HasOne(b => b.Tag)
                .WithMany()
                .HasForeignKey(b => b.TagId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

        // internal Task SaveChangesAsync()
        // {
        //     throw new NotImplementedException();
        // }


        //public DbSet<Tag> Tags { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Resource>()
        //        .Property(r => r.Tags)
        //        .HasConversion(
        //            tags => JsonConvert.SerializeObject(tags), // Or any other JSON serialization library
        //            value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value) // Or any other deserialization logic
        //        );

        //    base.OnModelCreating(modelBuilder);
    }

}


