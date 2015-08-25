using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MetroLogWebTarget.Core.Caching;
using MetroLogWebTarget.Data;
using MetroLogWebTarget.Domain;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Service
{
    public partial class UserStore<T> :
        IUserStore<T, int>,
        IUserClaimStore<T, int>,
        IUserRoleStore<T, int>,
        IUserPasswordStore<T, int>,
        IUserSecurityStampStore<T, int> where T : User
    {

        #region Fields

        private bool _disposed;

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _userRoleRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public UserStore(ICacheManager cacheManager,
            IRepository<User> userRepository,
            IRepository<Role> userRoleRepository)
        {
            this._cacheManager = cacheManager;
            this._userRepository = userRepository;
            this._userRoleRepository = userRoleRepository;
        }

        #endregion

        #region IUserClaimStore

        public Task AddClaimAsync(T user, Claim claim)
        {
            this.ThrowIfDisposed();
            if ((object)user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var userCtx = _userRepository.GetById(user.Id);//todo try Attach
            userCtx.Claims.Add(new UserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });
            return Task.Run(() => _userRepository.Update(userCtx));
        }

        public Task<IList<Claim>> GetClaimsAsync(T user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            IList<Claim> result = user.Claims.Select(userClaim => new Claim(userClaim.ClaimType, userClaim.ClaimValue)).ToList();

            return Task.FromResult(result);
        }

        public Task RemoveClaimAsync(T user, Claim claim)
        {
            this.ThrowIfDisposed();
            if ((object)user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var userCtx = _userRepository.GetById(user.Id);//todo try Attach
            foreach (var identityUserClaim in user.Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToList())
            {
                userCtx.Claims.Remove(identityUserClaim);
            }
            return Task.Run(() => _userRepository.Update(userCtx));
        }

        #endregion

        public Task CreateAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Run(() => _userRepository.Insert(user));
        }

        public Task DeleteAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (user.IsSystemAccount)
                throw new Exception("不能删除内置管理员账户");

            return Task.Run(() => _userRepository.Delete(user));
        }

        public Task<T> FindByIdAsync(int userId)
        {
            return Task.FromResult(_userRepository.GetById(userId) as T);
        }

        public Task<T> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var user = _userRepository.Table.FirstOrDefault(c => c.UserName == userName) as T;
            return Task.FromResult(user);
        }

        public Task UpdateAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Run(() => _userRepository.Update(user));
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

        #region Role

        public Task AddToRoleAsync(T user, string roleName)
        {
            var userCtx = _userRepository.GetById(user.Id);
            var role = _userRoleRepository.Table.FirstOrDefault(ur => ur.Name == roleName);

            if (role == null) throw new Exception("角色不存在");
            userCtx.Roles.Add(role);
            return Task.Run(() => _userRepository.Update(userCtx));
        }

        public Task RemoveFromRoleAsync(T user, string roleName)
        {
            var userCtx = _userRepository.GetById(user.Id);
            var role = _userRoleRepository.Table.FirstOrDefault(ur => ur.Name == roleName);

            if (role == null) throw new Exception("角色不存在");
            userCtx.Roles.Remove(role);
            return Task.Run(() => _userRepository.Update(userCtx));
        }

        public Task<IList<string>> GetRolesAsync(T user)
        {
            var list = user.Roles.Select(r => r.Name).ToList() as IList<string>;
            return Task.FromResult(list);
        }

        public Task<bool> IsInRoleAsync(T user, string roleName)
        {
            bool inRole = user.Roles.Any(r => r.Name == roleName);
            return Task.FromResult(inRole);
        }

        #endregion

        #region PasswordHash

        public Task SetPasswordHashAsync(T user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;
            return Task.Run(() => _userRepository.Update(user));
        }

        public Task<string> GetPasswordHashAsync(T user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(T user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        #endregion

        #region SecurityStamp

        public Task SetSecurityStampAsync(T user, string stamp)
        {
            this.ThrowIfDisposed();
            if ((object)user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;
            return Task.Run(() => _userRepository.Update(user));
        }

        public Task<string> GetSecurityStampAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion
    }
}