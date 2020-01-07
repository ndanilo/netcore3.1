using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Interfaces.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Identity;

namespace Infra.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSets

        public new DbSet<User> Users { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public new DbSet<RoleClaim> RoleClaims { get; set; }
        public new DbSet<UserClaim> UserClaims { get; set; }
        public new DbSet<UserLogin> UserLogins { get; set; }
        public new DbSet<UserRole> UserRoles { get; set; }
        public new DbSet<UserToken> UserTokens { get; set; }
        public DbSet<ApplicationCredential> ApplicationCredentials { get; set; }

        #endregion

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            #region  CustomTableNames

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserToken>().ToTable("UserTokens");
            modelBuilder.Entity<ApplicationCredential>().ToTable("ApplicationCredentials");

            #endregion

            #region Conversions

            //modelBuilder.Entity<TransferNote>()
            //    .Property(c => c.Description)
            //    .HasColumnType("varchar(255)")
            //    .IsRequired()
            //    .HasConversion<string>();

            #endregion

            #region ComposedPrimaryKeys

            #endregion

            #region Indexes

            modelBuilder.Entity<ApplicationCredential>()
                .HasIndex(ac => ac.ApiKey)
                .IsUnique();

            #endregion

            #region Relationships

            //builder.Entity<User>()
            //    .HasMany(uc => uc.UserCompanies)
            //    .WithOne(u => u.User);

            #endregion
        }


        private const string _isActiveProperty = nameof(IEntity.Active);
        private static readonly System.Reflection.MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).MakeGenericMethod(typeof(bool));

        private static LambdaExpression GetIsDeletedRestriction(System.Type type)
        {
            var param = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, param, Expression.Constant(_isActiveProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(true));
            var lambda = Expression.Lambda(condition, param);
            return lambda;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Expression<Func<IEntity, bool>> virtualDeletedFilter = e => e.Active == true;

            var entities = modelBuilder.Model.GetEntityTypes().Where(e => typeof(IEntity).IsAssignableFrom(e.ClrType));
            foreach (IMutableEntityType entity in entities)
            {
                entity.SetQueryFilter(GetIsDeletedRestriction(entity.ClrType));
            }

            base.OnModelCreating(modelBuilder);

            ConfigureEntities(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Active = true;
                        entry.Entity.CreatedAt = entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        if (entry.Entity.IsVirtualDeleted)
                        {
                            entry.State = EntityState.Modified;
                            entry.Entity.Active = false;
                        }
                        break;
                }
            }
        }
    }
}
