using System;
using Microsoft.EntityFrameworkCore;
using Iconic.Models.AppSetting;

namespace Iconic.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.Migrate();
            if (!Database.EnsureCreated()) return;

            SaveChangesAsync();
        }

        #region Icon

        public DbSet<Models.Icon.Icon> Icons { get; set; }

        #endregion

        #region User

        public DbSet<Models.User.User> Users { get; set; }

        #endregion

        #region AppSettings

        public DbSet<AppSetting> AppSettings { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            optionsBuilder.UseSqlite("Data Source=Iconic.db;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Icon

            modelBuilder.Entity<Models.Icon.Icon>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Date)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            #endregion

            #region User

            modelBuilder.Entity<Models.User.User>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            #endregion

            #region AppSetting

            modelBuilder.Entity<AppSetting>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.IsEditable)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}