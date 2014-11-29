using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data
{
    /// <summary>
    /// DbContext接口
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// DbSet
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>DbSet</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        void Dispose();
    }
}