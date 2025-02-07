using Eshop.Data.Ineterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext eshopContext;
        protected DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            eshopContext = context;

            dbSet = eshopContext.Set<TEntity>();
        }

        void IRepository<TEntity>.Delete(int id)
        {
            TEntity entity = dbSet.Find(id);
            try
            {
                dbSet.Remove(entity);
                eshopContext.SaveChanges();
            }
            catch (Exception)
            {
                eshopContext.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }

        TEntity IRepository<TEntity>.FindById(int id)
        {
            return dbSet.Find(id);
        }

        List<TEntity> IRepository<TEntity>.GetAll()
        {
            return dbSet.ToList();
        }

        void IRepository<TEntity>.Insert(TEntity entity)
        {
            dbSet.Add(entity);
            eshopContext.SaveChanges();
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            if (dbSet.Contains(entity))
                dbSet.Update(entity);
            else
                dbSet.Add(entity);

            eshopContext.SaveChanges();
        }
    }
}
