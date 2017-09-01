using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public abstract class BaseRepository<TEntity, TIdType> : IBaseRepository<TEntity>
        where TEntity : class, IBaseEntity<TIdType>
    {
        protected LearnToLearnContext context;

        public BaseRepository(LearnToLearnContext context)
        {
            this.context = context;
        }

        public List<TEntity> GetAll(Func<TEntity, bool> filter = null)
        {
            if (filter != null)
            {
                return context.Set<TEntity>().Where(filter).ToList();
            }

            return context.Set<TEntity>().ToList();
        }

        public void Create(TEntity item)
        {
            context.Set<TEntity>().Add(item);
        }

        public void Update(TEntity item, Func<TEntity, bool> findByPredicate)
        {
            var local = context.Set<TEntity>()
                         .Local
                         .FirstOrDefault(findByPredicate);
            context.Entry(item).State = EntityState.Modified;
        }

        public void DeleteById(int id)
        {
            TEntity dbItem = context.Set<TEntity>().Find(id);
            if (dbItem != null)
            {
                context.Set<TEntity>().Remove(dbItem);
            }
        }

        public void Save(TEntity item)
        {
            if (typeof(TIdType) == typeof(int))
            {
                int id = Convert.ToInt32(item.Id);
                if (id == 0)
                {
                    Create(item);
                }
                else
                {
                    Update(item, x => Convert.ToInt32(x.Id) == id);
                }
            }

            if (typeof(TIdType) == typeof(string))
            {
                string id = Convert.ToString(item.Id);
                if (id == null)
                {
                    Create(item);
                }
                else
                {
                    Update(item, x => Convert.ToString(x.Id) == id);
                }
            }
        }

        public void DeleteByPredicate(Func<TEntity, bool> filter)
        {
            IEnumerable<TEntity> itemsToRemove = context.Set<TEntity>().Where(filter);
            context.Set<TEntity>().RemoveRange(itemsToRemove);
        }
    }
}
