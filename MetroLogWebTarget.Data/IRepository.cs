using System;
using System.Collections.Generic;
using System.Linq;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data
{
    public interface IRepository<T> : IDisposable
    {
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
