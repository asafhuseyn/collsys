using CollSys.Matm.Kitabxana.BusinessLogic.Services.Base;
using CollSys.Matm.Kitabxana.DataAccess.Interfaces.Base;
using CollSys.Matm.Kitabxana.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CollSys.Matm.Kitabxana.BusinessLogic.Managers.Base
{
    public class ManagerRepository<TEntity, TDal> : IServiceRepository<TEntity>
         where TEntity : class, IEntity, new()
         where TDal : class, IDalRepository<TEntity>, new()
    {
        protected TDal dal = new TDal();

        public TEntity Create(TEntity model)
        {
            return dal.Create(model);
        }

        public TEntity Delete(TEntity model)
        {
            return dal.Delete(model);
        }

        public DbContext Instance(bool isNew = false)
        {
            return dal.Instance(isNew);
        }

        public List<TEntity> Read(Expression<Func<TEntity, bool>> filter = null)
        {
            return dal.Read(filter);
        }

        public void Save()
        {
            dal.Save();
        }

        public TEntity Take(Expression<Func<TEntity, bool>> filter)
        {
            return dal.Take(filter);
        }
        
        public TEntity TakeFirst(Expression<Func<TEntity, bool>> filter)
        {
            return dal.TakeFirst(filter);
        }

        public TEntity Update(TEntity model)
        {
            return dal.Update(model);
        }

        public void Detach(TEntity model)
        {
            dal.Detach(model);
        }
    }
}
