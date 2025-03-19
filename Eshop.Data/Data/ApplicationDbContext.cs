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
            var product = new Product
            {
                ProductId = 17,
                Code = "something",
                Hidden = false,
                Price = 500,
                ImagesCount = 2,
                Stock = 11,
                Url = "zaves-sense-zeleny",
                Title = "Závěs Sense, zelený",
                ShortDescription = "Lorem ipsum dolor sit amet. Qui cumque harum a iure sapiente hic debitis blanditiis At vero voluptatem sit molestiae dolores et quas dolorum. Qui facilis impedit et ipsam sint sed numquam necessitatibus.",
                Description = "<p>Lorem ipsum dolor sit amet. Qui cumque harum a iure sapiente hic debitis blanditiis At vero voluptatem sit molestiae dolores et quas dolorum. Qui facilis impedit et ipsam sint sed numquam necessitatibus ex autem omnis et saepe odio? Et cumque autem aut quas fugiat et animi saepe et nobis tempore et magni facere. </p><p>Non quisquam corrupti et dolorem provident ut cumque porro et error laudantium et labore autem. A quod nobis aut labore modi ut nemo modi in fugit dolorum non dolores quia non libero laudantium? Aut nemo maxime sed suscipit aspernatur aut dolores amet in iusto nostrum. </p>"
            };

            builder.Entity<Product>().HasData(
                product
                );

            builder.Entity<CategoryProduct>().HasData(
                new CategoryProduct { CategoryId = 1, ProductId = 17},
                new CategoryProduct { CategoryId = 3, ProductId = 17}
                );

            builder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Title = "Doplňky", Url = "doplnky", OrderNo = 1, Hidden = false },
                new Category { CategoryId = 2, Title = "Stolování", Url = "stolovani", OrderNo = 4, Hidden = false },

                new Category { CategoryId = 3, ParentCategoryId = 1, Title = "Závěsy", Url = "zavesy", OrderNo = 2, Hidden = false },
                new Category { CategoryId = 4, ParentCategoryId = 1, Title = "Květináče", Url = "kvetinace", OrderNo = 3, Hidden = false },
                new Category { CategoryId = 5, ParentCategoryId = 2, Title = "Hrníčky", Url = "hrnicky", OrderNo = 5, Hidden = false });
        }
    }
}
