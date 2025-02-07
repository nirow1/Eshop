using Eshop.Data.Ineterfaces;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
        public Product FindByUrl(string url)
        {
            var product = dbSet.SingleOrDefault(p => p.Url == url && !p.Hidden);
            return product;
        }
    }
}
