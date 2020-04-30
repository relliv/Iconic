using System;
using Microsoft.EntityFrameworkCore;
using Iconic.Models.AppSetting;
using Iconic.Models.Color.Entities;
using Iconic.Models.Icon.Entities;
using Iconic.Models.User.Entities;

namespace Iconic.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext()
        {

            if (!Database.EnsureCreated()) return;

            // save default settings
            AppSettings.Add(new AppSetting
            {
                SettingName = "IconsSource",
                Value = "https://materialdesignicons.com/api",
                IsEditable = true,
                DefaultValue = "https://materialdesignicons.com/api"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "IconsGUID",
                Value = "38EF63D0-4744-11E4-B3CF-842B2B6CFE1B",
                IsEditable = true,
                DefaultValue = "38EF63D0-4744-11E4-B3CF-842B2B6CFE1B"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "IconsLastUpdate",
                Value = "12.12.2012 12:12:12",
                IsEditable = false,
                DefaultValue = "12.12.2012 12:12:12"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "ColorPaletteSource",
                Value = "https://raw.githubusercontent.com/Jam3/nice-color-palettes/master/1000.json",
                IsEditable = true,
                DefaultValue = "https://raw.githubusercontent.com/Jam3/nice-color-palettes/master/1000.json"
            });

            SaveChangesAsync();
        }

        #region Icon

        public DbSet<Icon> Icons { get; set; }

        #endregion

        #region Color

        public DbSet<ColorPalette> ColorPalettes { get; set; }

        #endregion

        #region User

        public DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<Icon>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.GUId).IsUnique();
                entity.Property(x => x.Date)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            #endregion

            #region Color

            modelBuilder.Entity<ColorPalette>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            #endregion

            #region User

            modelBuilder.Entity<User>(entity =>
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