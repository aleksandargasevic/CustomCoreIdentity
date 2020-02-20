using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomCoreIdentity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CustomCoreIdentity.Data {
    public class AppUserManager : UserManager<User> {
        private readonly RoleManager<Role> _roleManager;

        public AppUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<User>> logger,
            RoleManager<Role> roleManager)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
                services, logger) {
            _roleManager = roleManager;
        }

        public async Task<IList<Role>> GetModelRolesAsync(User user)
        {
            IList<string> roleNames = await base.GetRolesAsync(user);

            var identityRoles = new List<Role>();
            foreach (var roleName in roleNames)
            {
                Role role = await _roleManager.FindByNameAsync(roleName);
                identityRoles.Add(role);
            }

            return identityRoles;
        }
    }
}
