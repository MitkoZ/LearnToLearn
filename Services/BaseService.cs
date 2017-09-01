using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public abstract class BaseService<TEntity, TIdType, TRepository, TUnitOfWork>
        where TEntity : class, IBaseEntity<TIdType>
        where TRepository : BaseRepository<TEntity, TIdType>
        where TUnitOfWork : IUnitOfWork
    {
        #region Constructors and fields
        protected IValidationDictionary validationDictionary;
        protected TUnitOfWork unitOfWork;
        protected TRepository repository;
        public BaseService(IValidationDictionary validationDictionary, TRepository repository, TUnitOfWork unitOfWork)
        {
            this.validationDictionary = validationDictionary;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }
        #endregion

        public bool PreValidate()
        {
            if (validationDictionary.isValid)
            {
                return true;
            }
            return false;
        }

        public List<TEntity> GetAll(Func<TEntity, bool> filter = null)
        {
            return repository.GetAll(filter);
        }

        public bool Save(TEntity entity)
        {
            repository.Save(entity);
            return unitOfWork.Save() > 0;
        }

        public bool DeleteById(int id)
        {
            repository.DeleteById(id);
            return unitOfWork.Save() > 0;
        }

        public bool DeleteByPredicate(Func<TEntity, bool> filter)
        {
            repository.DeleteByPredicate(filter);
            return unitOfWork.Save() > 0;
        }
    }
}
