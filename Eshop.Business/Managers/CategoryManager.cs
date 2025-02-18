using Eshop.Business.Interfaces;
using Eshop.Data.Ineterfaces;
using Eshop.Data.Models;


namespace Eshop.Business.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public CategoryManager(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public void UpdateProductCategories(int productId, IEnumerable<int> categories)
        {
            var product = productRepository.FindById(productId)
                ?? throw new ArgumentNullException($"Produkt {productId} nebyl nalezen");

            var currentCategories = product.CategoryProducts.Select(cp => cp.CategoryId);
            var removeCategories = currentCategories.Except(categories);
            var addCategories = categories.Except(currentCategories);

            foreach (var categoryId in removeCategories)
            {
                var toRemove = product.CategoryProducts.Where(cp => cp.CategoryId == categoryId).SingleOrDefault();

                product.CategoryProducts.Remove(toRemove);
            }

            foreach(var categoryId in addCategories)
            {
                var toAdd = new CategoryProduct()
                {
                    CategoryId = categoryId,
                    ProductId = product.ProductId
                };
                product.CategoryProducts.Add(toAdd);
            }

            productRepository.Update(product);
        }

        public List<Category> GetAll(bool withHidden = false)
        {
            return categoryRepository.GetAll(withHidden);
        }
    }
}
