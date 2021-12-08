using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sara.Core;
using System;
using System.Linq;

namespace Sara.Data
{
    public class SaraContext : IdentityDbContext<SaraUser, SaraRole, Guid>
    {
        public SaraContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SaraUser>(entity => {
                entity.Property(m => m.Email).HasMaxLength(127);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(127);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(127);
                entity.Property(m => m.UserName).HasMaxLength(127);
                entity.Property(m => m.PhoneNumber).IsRequired();
                entity.Property(m => m.FirebaseToken).HasColumnType("TEXT");
            });
            builder.Entity<SaraRole>(entity => {
                entity.Property(m => m.Id).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);
                entity.Property(m => m.NormalizedName).HasMaxLength(127);
            });
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });
            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });
            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(110);
                entity.Property(m => m.Name).HasMaxLength(127);
            });

            builder.Entity<Campaign>(entity =>
            {
                entity.Property(p => p.Content).HasColumnType("TEXT");

                entity.HasIndex(i => i.Tag).HasName("UX_Tag").IsUnique();

                entity.HasMany(x => x.Campaigns)
                .WithOne(x => x.Challenge);
            });

            builder.Entity<Customer>(entity => 
            {
                entity.Property(x => x.LogoAvatar).HasColumnType("TEXT");
                entity.HasIndex(i => i.Tag).HasName("UX_Customer_Tag").IsUnique();
            });

            builder.Entity<Voucher>(entity =>
            {
                entity.Property(x => x.WasRewarded).HasDefaultValue(false);
                entity.Property(x => x.RewardedAt).HasDefaultValue(null);
            });

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.Name.Equals("Tag"))
                {
                    property.SetMaxLength(8);
                    property.IsNullable = false;
                }
                else
                    property.SetMaxLength(127);
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<GenderIdentity> GenderIdentities { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Sexuality> Sexualities { get; set; }


        //public SaraContext() { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseMySql("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_sara;Uid=hartbroot;Pwd=D45m7eba;");

        //}


    }
}
