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
    }
}
