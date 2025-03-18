using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Data.Ineterfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        List<Category> GetAll(bool withHidden);

        List<Category> GetRootCategories();
    }
}
