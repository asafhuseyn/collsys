using CollSys.Matm.Kitabxana.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CollSys.Matm.Kitabxana.BusinessLogic.Services.Base
{
    public interface IServiceRepository<TEntity> where TEntity : class, IEntity, new()
    {
        TEntity Create(TEntity model);
        List<TEntity> Read(Expression<Func<TEntity, bool>> filter = null);
        TEntity Update(TEntity model);
        TEntity Delete(TEntity model);

        TEntity Take(Expression<Func<TEntity, bool>> filter);

        TEntity TakeFirst(Expression<Func<TEntity, bool>> filter);

        DbContext Instance(bool isNew = false);
        void Save();
        void Detach(TEntity model);
    }
}
