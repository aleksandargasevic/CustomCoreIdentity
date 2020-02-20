using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CustomCoreIdentity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomCoreIdentity.Data
{
    public class CustomUserStore : IUserPasswordStore<User>, IUserEmailStore<User>, IUserRoleStore<User>, IUserClaimStore<User>
    {
        private readonly ApplicationDbContext _context;

        public CustomUserStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Add(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not create user {user.Email}." });
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var userFromDb = await _context.Users.FindAsync(user.Id);
            _context.Remove(userFromDb);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete user {user.Email}." });
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)), cancellationToken);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(normalizedUserName.ToLower()),
                cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Email = userName;
            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Update(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not update user {user.Email}." });
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(normalizedEmail),
                cancellationToken);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));

            var roleEntity = await _context.Roles.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                    "Role Not Found", roleName));
            }

            var ur = new UserRole(){ UserId = user.Id, RoleId = roleEntity.Id };
            _context.UserRoles.Add(ur);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            var userRole = await _context.UserRoles.SingleOrDefaultAsync(r => r.UserId == user.Id && r.RoleId == role.Id);
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var query = from userRole in _context.UserRoles
                where userRole.UserId.Equals(user.Id)
                join role in _context.Roles on userRole.RoleId equals role.Id
                select role.Name;
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
            if (role != null)
            {
                var userId = user.Id;
                var roleId = role.Id;
                return await _context.UserRoles.AnyAsync(ur => ur.RoleId.Equals(roleId) && ur.UserId.Equals(userId), cancellationToken);
            }
            return false;
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));

            var query = from user in _context.Users
                join role in _context.Roles on user.Id equals role.Id
                where role.Name == roleName
                select user;

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return await _context.UserClaims.Where(uc => uc.UserId == user.Id).Select(s => s.ToClaim()).ToListAsync(cancellationToken);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken) {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claims == null)
                throw new ArgumentNullException("claims");
            var instance = Activator.CreateInstance<IdentityUserClaim<Guid>>();
            foreach (var claim in claims) {
                instance.UserId = user.Id;
                instance.ClaimType = claim.Type;
                instance.ClaimValue = claim.Value;
                var userClaim = instance;
                _context.UserClaims.Add(userClaim);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken) {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claims == null)
                throw new ArgumentNullException("claims");

            var userClaims = new List<IdentityUserClaim<string>>(0);
            foreach (var claim in claims) {
                var claimDb =
                    await _context.UserClaims.FirstOrDefaultAsync(
                            uc => uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value && uc.UserId == user.Id);
                if (claimDb != null)
                {
                    _context.Remove(claimDb);
                    _context.UserClaims.Remove(claimDb); 
                }
            }
            _context.SaveChanges();
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken) {
            if (claim == null)
                throw new ArgumentNullException("user");

            var users = from user in _context.Users
                where _context.UserClaims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value)
                select user;
            return await users.ToListAsync(cancellationToken);
        }
    }
}
