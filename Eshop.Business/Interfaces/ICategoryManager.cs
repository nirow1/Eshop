using Eshop.Data.Models;

namespace Eshop.Business.Interfaces
{
    public interface ICategoryManager
    {
        void UpdateProductCategories(int productId, IEnumerable<int> categories);
        List<Category> GetAll(bool withHidden = false);
        List<Category> GetRootCategories();
        Category GetCategoryById(int id);
    }
}
