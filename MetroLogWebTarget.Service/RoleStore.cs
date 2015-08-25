using System;
using System.Linq;
using System.Threading.Tasks;
using MetroLogWebTarget.Data;
using MetroLogWebTarget.Domain;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Service
{
    public class RoleStore<T>:IRoleStore<T,int> where T:Role
    {
        private bool _disposed;
        private readonly IRepository<Role> _roleRepository;

        public RoleStore(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task CreateAsync(T role)
        {
            if(role == null)
                throw new ArgumentNullException("role");

            return Task.Run(() => _roleRepository.Insert(role));
        }

        public Task UpdateAsync(T role)
        {
            if(role == null)
                throw new ArgumentNullException("role");

            return Task.Run(() => _roleRepository.Update(role));
        }

        public Task DeleteAsync(T role)
        {
            if(role == null)
                throw new ArgumentNullException("role");

            return Task.Run(() => _roleRepository.Delete(role));
        }

        public Task<T> FindByIdAsync(int roleId)
        {
            return Task.FromResult(_roleRepository.GetById(roleId) as T);
        }

        public Task<T> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return null;

            var role = _roleRepository.Table.FirstOrDefault(r => r.Name == roleName) as T;
            return Task.FromResult(role);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }
    }
}
