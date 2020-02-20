using System;
using System.Collections.Generic;
using System.Text;
using CustomCoreIdentity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomCoreIdentity.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<IdentityUserClaim<Guid>> UserClaims { get; set; }
        //public DbSet<IdentityUserToken<Guid>> UserTokens { get; set; }
        public DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; set; }

    }
}
