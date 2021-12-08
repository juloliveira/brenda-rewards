using Carol.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Carol.Data
{
    public class CarolContext : DbContext
    {
        public CarolContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.Property(x => x.FirebaseToken).HasColumnType("TEXT");
            });

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                property.SetMaxLength(127);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        //public CarolContext() { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseMySql("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_carol;Uid=hartbroot;Pwd=D45m7eba;");

        //}
    }
}
