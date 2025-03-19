using Eshop.Data.Classes;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Data.Ineterfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product FindByUrl(string url);
        List<Product> FindAllBy(
            string searchPhrase = null,
            int? categoryId = null,
            decimal startPrice = 0,
            decimal endPrice = int.MaxValue,
            bool onlyInStock = false,
            bool onSale = false,
            string orderBy = OrderProductBy.Newest,
            int count = int.MaxValue,
            bool withoutHidden = true
            );
    }
}
