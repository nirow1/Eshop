using Eshop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().Property(x => x.Price).HasColumnType("decimal(10,2)");
            builder.Entity<Product>().Property(x => x.OldPrice).HasColumnType("decimal(10,2)");

            builder.Entity<CategoryProduct>().HasKey(cp => new 
                {
                    cp.CategoryId,
                    cp.ProductId
                });

            builder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId);

            builder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Category)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId);

            AddTestingData(builder);
        }

        private void AddTestingData(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Title = "Doplňky", Url = "doplnky", OrderNo = 1, Hidden = false },
                new Category { CategoryId = 2, Title = "Stolování", Url = "stolovani", OrderNo = 4, Hidden = false },

                new Category { CategoryId = 3, ParentCategoryId = 1, Title = "Závěsy", Url = "zavesy", OrderNo = 2, Hidden = false },
                new Category { CategoryId = 4, ParentCategoryId = 1, Title = "Květináče", Url = "kvetinace", OrderNo = 3, Hidden = false },
                new Category { CategoryId = 5, ParentCategoryId = 2, Title = "Hrníčky", Url = "hrnicky", OrderNo = 5, Hidden = false });
        }
    }
}
