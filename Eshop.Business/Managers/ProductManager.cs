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
        private readonly IProductRepository productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Product FindProductById(int id)
        {
            return productRepository.FindById(id);
        }

        public Product FindProductByUrl(string url)
        {
            return productRepository.FindByUrl(url);
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
                productRepository.Delete(oldProduct.ProductId);
            }
            catch (Exception)
            {
                oldProduct.CategoryProducts.Clear();
                oldProduct.Hidden = true;
                productRepository.Update(oldProduct);
            }
        }

        public void SaveProduct(Product product)
        {
            var oldProduct  = productRepository.FindById(product.ProductId);

            if (product.ProductId != 0)
                product.ProductId = 0;

            productRepository.Insert(product);

            if (oldProduct != null)
                CleanProduct(oldProduct);
        }
    }
}
