using Eshop.Business.Interfaces;
using Eshop.Business.Classes;
using Eshop.Data.Ineterfaces;
using Eshop.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Eshop.Business.Managers
{
    public class ProductManager : IProductManager
    {
        private const string ProductImagePath = "wwwroot/Images/Products/";
        private const int ProductImageHeight = 1080;
        private const int ProductThumbnailWidth = 480;

        private readonly IImageManager imageManager;

        private readonly IProductRepository productRepository;

        public ProductManager(IProductRepository productRepository, IImageManager imageManager)
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

        public void CleanProduct(Product oldProduct, bool removeImages = false)
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

            if (removeImages)
            {
                int imagesCount = oldProduct.ImagesCount;
                RemoveThumbnailFile(oldProduct.ProductId);
                for (int i = 0; i < imagesCount; i++)
                    RemoveImageFile(oldProduct.ProductId, i);
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
                result = Path.Combine(ProductImagePath, $"{result}.{IProductManager.ProductImageExtension}");

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

        private void RemoveThumbnailFile(int productId)
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

            if(File.Exists(oldThumbnailPath))
                File.Move(oldThumbnailPath, newThumbnailPath);

            for (int i = 0; i < imagesCount; i++)
                RenameImage(oldProductId, i, productId, i);
        }
        #endregion

        public void SaveProductImages(Product product, List<IFormFile> images, int? oldProductID, int? oldImagesCount)
        {
            if (images is null)
                throw new Exception("Nepodařilo se najít žádné obrázky");

            int imagesCount = 0;

            if (oldProductID.HasValue)
            {
                imagesCount = oldImagesCount.Value;
                RenameProductImages(oldProductID.Value, product.ProductId, imagesCount);
            }

            for (int i = 0; i < images.Count; i++)
            {
                if (images[i] is null || !images[i].ContentType.ToLower().Contains("image"))
                    continue;

                imageManager.SaveImage(
                    images[i],
                    GetImageFileName(product.ProductId, oldImagesCount.Value + i, full: false),
                    ImageExtension.Png,
                    height: ProductImageHeight);
                
                if(imagesCount == 0)
                {
                    imageManager.SaveImage(
                        images[i],
                        GetThumbnailFileName(product.ProductId, full: false),
                        ImageExtension.Png,
                        ProductThumbnailWidth);
                }
                imagesCount++;
            }
            product.ImagesCount = imagesCount;
            productRepository.Update(product);
        }

        public void RemoveProductImage(int productId, int imageIndex)
        {
            var product = productRepository.FindById(productId);
            if (imageIndex == 0)
            {
                RemoveThumbnailFile(productId);
                string secondImagePath = GetThumbnailFileName(product.ProductId);
                
                if (File.Exists(secondImagePath))
                {
                    string thumbnailFileName = GetThumbnailFileName(product.ProductId);
                    imageManager.ResizeImage(secondImagePath, thumbnailFileName, ProductThumbnailWidth);
                }
            }
            RemoveImageFile(productId, imageIndex);

            for (int i = imageIndex + 1; i < product.ImagesCount; i++)
                RenameImage(productId, i, productId, i - 1);

            product.ImagesCount--;
            productRepository.Update(product);
        }
    }
}
