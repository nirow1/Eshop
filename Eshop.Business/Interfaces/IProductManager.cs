using Eshop.Data.Classes;
using Eshop.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Business.Interfaces
{
    public interface IProductManager
    {

        public const string ProductImageExtension = "png";
        public const string ProductThumbnailExtension = "png";
        Product FindProductById(int id);

        Product FindProductByUrl(string url);

        bool[] FindAssignedCategoriesToProduct(
            List<Category> availableCategories,
            List<CategoryProduct> assignedCategories,
            bool[] postedCategories);

        List<Product> FindByCategoryId(int categoryId);
        
        List<Product> FindBySearchPhrase(string searchPhrase);
        
        List<Product> FindBy(
            string searchPhrase = null,
            int? categoryId = null,
            string orderBy = OrderProductBy.Newest,
            decimal startPrice = 0,
            decimal endPrice = int.MaxValue,
            bool onlyInStock = false,
            bool onSale = false,
            int count = int.MaxValue
            );

        void SaveProduct(Product product);
        void CleanProduct(Product oldProduct, bool removeImages = false);

        void SaveProductImages(Product product, List<IFormFile> images, int? oldProductId, int? oldImagesCount);

        void RemoveProductImage(int productId, int imageIndex);
    }
}
