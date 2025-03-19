using Eshop.Data.Classes;
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
        public List<Product> FindAllBy(string searchPhrase = null, int? categoryId = null, decimal startPrice = 0,
                                     decimal endPrice = int.MaxValue, bool onlyInStock = false, bool onSale = false,
                                     string orderBy = OrderProductBy.Newest, int count = int.MaxValue, bool withoutHidden = true)
        {
            var query = withoutHidden ? dbSet.Where(p => !p.Hidden) : dbSet;

            if (!string.IsNullOrWhiteSpace(searchPhrase))
                query = query.Where(p => p.Title.Contains(searchPhrase) ||
                p.ShortDescription.Contains(searchPhrase) ||
                p.Description.Contains(searchPhrase)
                );

            if(categoryId.HasValue)
                query = query.Where(p=>p.CategoryProducts.Any(cp => cp.CategoryId == categoryId));

            if (startPrice > 0)
                query = query.Where(p => p.Price >= startPrice);

            if (endPrice < int.MaxValue)
                query = query.Where(p => p.Price <= endPrice);

            if (onlyInStock)
                query = query.Where(p => p.Stock > 0);

            if (onSale)
                query = query.Where(p => p.OldPrice != null);

            query = orderBy switch
            {
                OrderProductBy.LowestPrice => query.OrderBy(p => p.Price),
                OrderProductBy.HighestPrice => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.ProductId)
            };

            var list = query.ToList();

            if(orderBy == OrderProductBy.Rating)
                list = list.OrderByDescending(p => p.Rating).Take(count).ToList();

            return list;
        }
    }
}
