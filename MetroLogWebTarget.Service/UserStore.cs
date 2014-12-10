using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Galenical.Core.Caching;
using MetroLogWebTarget.Core.Caching;
using MetroLogWebTarget.Data;
using MetroLogWebTarget.Domain;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Service
{
    /// <summary>
    /// User service
    /// </summary>
    public partial class UserStore<T> :
        IUserStore<T, int>,
        IUserClaimStore<T, int>,
        IUserRoleStore<T, int>,
        IUserPasswordStore<T, int>,
        IUserSecurityStampStore<T, int> where T : User
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string CUSTOMERROLES_ALL_KEY = "LCM.userrole.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "LCM.userrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "LCM.userrole.";

        #endregion

        #region Fields

        private bool _disposed;

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="userRepository">User repository</param>
        /// <param name="userRoleRepository">User role repository</param>
        public UserStore(ICacheManager cacheManager,
            IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository)
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
                throw new Exception("����ɾ�����ù���Ա�˻�");

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

            if (role == null) throw new Exception("��ɫ������");
            userCtx.UserRoles.Add(role);
            return Task.Run(() => _userRepository.Update(userCtx));
        }

        public Task RemoveFromRoleAsync(T user, string roleName)
        {
            var userCtx = _userRepository.GetById(user.Id);
            var role = _userRoleRepository.Table.FirstOrDefault(ur => ur.Name == roleName);

            if (role == null) throw new Exception("��ɫ������");
            userCtx.UserRoles.Remove(role);
            return Task.Run(() => _userRepository.Update(userCtx));
        }

        public Task<IList<string>> GetRolesAsync(T user)
        {
            var list = user.UserRoles.Select(r => r.Name).ToList() as IList<string>;
            return Task.FromResult(list);
        }

        public Task<bool> IsInRoleAsync(T user, string roleName)
        {
            bool inRole = user.UserRoles.Any(r => r.Name == roleName);
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