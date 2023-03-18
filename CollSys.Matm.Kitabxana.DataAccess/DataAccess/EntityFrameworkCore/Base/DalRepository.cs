using CollSys.Matm.Kitabxana.DataAccess.Connections.EntityFrameworkCore;
using CollSys.Matm.Kitabxana.DataAccess.Interfaces.Base;
using CollSys.Matm.Kitabxana.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CollSys.Matm.Kitabxana.DataAccess.DataAccess.EntityFrameworkCore.Base
{
    public class DalRepository<TEntity, TContext> : IDalRepository<TEntity>
         where TEntity : class, IEntity, new()
         where TContext : DbContext, new()
    {
        TContext context;
        public TEntity Create(TEntity model)
        {
            context.Entry(model).State = EntityState.Added;
            return model;
        }

        public TEntity Delete(TEntity model)
        {
            context.Entry(model).State = EntityState.Deleted;
            return model;
        }

        public DbContext Instance(bool isNew = false)
        {
            return isNew == true ? context = new TContext() : context;
        }

        public List<TEntity> Read(Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public TEntity Take(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public TEntity TakeFirst(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return context.Set<TEntity>().FirstOrDefault(filter);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TEntity Update(TEntity model)
        {
            context.Entry(model).State = EntityState.Modified;
            return model;
        }

        public void Detach(TEntity model)
        {
            context.Entry(model).State = EntityState.Detached;
        }
    }
}
