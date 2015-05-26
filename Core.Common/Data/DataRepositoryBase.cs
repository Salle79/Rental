using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Core.Common.Contracts;
using Core.Common.Utils;

namespace Core.Common.Data
{
    public abstract class DataRepositoryBase<T, U> : IDataRepository<T>
        where T : class, IIdentifiableEntity, new ()
        where U : DbContext, new ()
    {
        public abstract T AddEntity(U entityContext, T entity);
        public abstract T UpdateEntity( U entityContext, T entity);
        public abstract T GetEntity( U entityContext, int id);
        public abstract IEnumerable<T> GetEntity(U entityContext);



        #region IDataRepository<T> Members

        public T Add(T entity)
        {
            using (U entityContext = new U()) 
            {
                T addedEntity = AddEntity (entityContext, entity);
                entityContext.SaveChanges ();
                return addedEntity;
            }
        }

        public void Remove(T entity)
        {
            using ( U entityContext = new U()) 
            {
                entityContext.Entry <T> (entity).State = EntityState.Deleted;
                entityContext.SaveChanges ();
            }
        }

        public void Remove(int id)
        {
            using ( U entityContext = new U ()) 
            {
                T removedEntity = GetEntity (entityContext, id);
                entityContext.Entry<T>(removedEntity).State = EntityState.Deleted;
                entityContext.SaveChanges ();
            }
        }

        public T Update(T entity)
        {
            using (U entityContext = new U())
            {
                T updatedEntity = UpdateEntity(entityContext, entity);
                SimpleMapper.PropertyMap<T, T> (entity, updatedEntity);
                entityContext.SaveChanges ();
                return updatedEntity;                      
            }
        }

        public T Get(int id)
        {
            using (U entityContext = new U()) 
            {
                return GetEntity ( entityContext, id);
            }
        }

        public IEnumerable<T> Get()
        {
            using (U entityContext = new U ())
            {
                return (GetEntity (entityContext)).ToList ().ToArray ();
            }
        }

        #endregion
    }
}
