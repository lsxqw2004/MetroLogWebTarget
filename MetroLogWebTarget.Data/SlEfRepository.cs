using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data
{
    public class SlEfRepository<T> : IRepository<T> where T : BaseEntity
    {

        public readonly IDbContext Context;
        private IDbSet<T> _entities;

        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = Context.Set<T>();
                return _entities;
            }
        }

        public SlEfRepository(MetroLogContext context)
        {
            Context = context;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Add(entity);

            this.Context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Remove(entity);

            this.Context.SaveChanges();
        }

        public IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public IQueryable<T> TableNoTracking
        {
            get { return Entities.AsNoTracking(); }
        }
    }
}
