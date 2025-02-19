using Eshop.Business.Interfaces;
using Eshop.Data.Ineterfaces;
using Eshop.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Business.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository ProductRepository;
        public ProductManager(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }

        public Product FindProductById(int id)
        {
            return ProductRepository.FindById(id);
        }

        public Product FindProductByUrl(string url)
        {
            return ProductRepository.FindByUrl(url);
        }

        public bool[] FindAssignedCategoriesToProduct(List<Category> availableCategories, List<CategoryProduct> assignedCategories, bool[] postedCategories)
        {
            for (int p = 0; p < assignedCategories.Count; p++)
            {
                for(int a = 0; a < availableCategories.Count; a++)
                {
                    if (availableCategories[a].CategoryId == assignedCategories[p].CategoryId)
                    {
                        postedCategories.SetValue(true, a);
                        break;
                    }
                }
            }
            return postedCategories;
        }

        public void CleanProduct(Product oldProduct)
        {
            try
            {
                ProductRepository.Delete(oldProduct.ProductId);
            }
            catch (Exception)
            {
                oldProduct.CategoryProducts.Clear();
                oldProduct.Hidden = true;
                ProductRepository.Update(oldProduct);
            }
        }

        public void SaveProduct(Product product)
        {
            var oldProduct  = ProductRepository.FindById(product.ProductId);

            if (oldProduct.ProductId != 0)
                product.ProductId = 0;

            if (oldProduct != null)
                CleanProduct(oldProduct);
        }
    }
}
