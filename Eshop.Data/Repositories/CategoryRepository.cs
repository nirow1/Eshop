using Eshop.Data.Ineterfaces;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Data.Repositories
{
    public class CategoryRepository: BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository( ApplicationDbContext context): base(context) { }

        public List<Category> GetAll(bool withHidden)
        {
            return withHidden ? GetAll() : dbSet.Where(c => !c.Hidden).ToList();
        }
    }
}
