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
        private const string ProductImagePath = "wwwroot/images/products/";
        private const int ProductImageHeight = 1080;
        private const int ProductThumbnailWidth = 480;

        private readonly IImageManager imageManager;

        private readonly IProductRepository productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this.imageManager = imageManager.ConfigureOutputPath(ProductImagePath);
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

        #region Private methods
        private void EnsureProductDirectoryCreated(int productId)
        {
            string path = Path.Combine(ProductImagePath, productId.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private string GetImageFileName(int productId, int imageIndex, bool full = true)
        {
            EnsureProductDirectoryCreated(productId);

            string result = Path.Combine(productId.ToString(), $"{productId}_{imageIndex}");

            if (full)
                result = Path.Combine(ProductImagePath, $"{result}.{IProductManager.ProductImageExtentions}");

            return result;
        }
        private string GetThumbnailFileName(int productId, bool full = true)
        {
            EnsureProductDirectoryCreated(productId);

            string result = Path.Combine(productId.ToString(), $"{productId}_thumb");

            if (full)
                result = Path.Combine(ProductImagePath, $"{result}.{IProductManager.ProductThumbnailExtension}");

            return result;
        }

        private void RemoveImageFile(int productId, int imageIndex)
        {
            string fileName = GetImageFileName(productId, imageIndex);
            if(File.Exists(fileName))
                File.Delete(fileName);
        }

        private void RemoveThubnailFile(int productId)
        {
            string thumbfileName = GetThumbnailFileName(productId);
            if(File.Exists(thumbfileName))
                File.Delete(thumbfileName);
        }

        private void RenameImage(int oldProductId, int oldImageIndex, int productId, int imageIndex)
        {
            string oldPath = GetImageFileName(oldProductId, oldImageIndex);
            string newPath = GetImageFileName(productId, imageIndex);
            if(File.Exists (oldPath))
                File.Move(oldPath, newPath);
        }

        private void RenameProductImages(int oldProductId, int productId, int imagesCount)
        {
            if (imagesCount == 0)
                return;

            string oldThumbnailPath = GetThumbnailFileName(oldProductId);
            string newThumbnailPath = GetThumbnailFileName(productId);

            if(File.Exists (oldThumbnailPath))
                File.Move(oldThumbnailPath, newThumbnailPath);

            for (int i = 0; i < imagesCount; i++)
                RenameImage(oldProductId, i, productId, i);
        }
        #endregion
    }
}
