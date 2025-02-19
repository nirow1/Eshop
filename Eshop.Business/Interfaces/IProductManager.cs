using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Business.Interfaces
{
    public interface IProductManager
    {
        Product FindProductById(int id);

        Product FindProductByUrl(string url);

        bool[] FindAssignedCategoriesToProduct(
            List<Category> availableCategories,
            List<CategoryProduct> assignedCategories,
            bool[] postedCategories);

        void SaveProduct(Product product);
        void CleanProduct(Product oldProduct);
    }
}
