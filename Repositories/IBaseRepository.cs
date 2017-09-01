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
    public interface IBaseRepository<TEntity>
    {
        List<TEntity> GetAll(Func<TEntity, bool> filter = null);

        void Save(TEntity item);

        void Create(TEntity item);

        void Update(TEntity item, Func<TEntity, bool> findByPredicate);
    }
}
