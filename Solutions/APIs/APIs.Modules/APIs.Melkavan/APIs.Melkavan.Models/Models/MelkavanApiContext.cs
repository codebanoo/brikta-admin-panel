using APIs.Melkavan.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Melkavan;
using VM.Melkavan.VM.Melkavan;

namespace APIs.Melkavan.Models
{
    public partial class MelkavanApiContext : DbContext
    {
        public MelkavanApiContext()
        {
        }

        public MelkavanApiContext(DbContextOptions<MelkavanApiContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(int.MaxValue);
        }

        #region Melkavan

        public virtual DbSet<Smses> Smses { get; set; }
        public virtual DbSet<Advertisement> Advertisement { get; set; }
        public virtual DbSet<AdvertisementTypes> AdvertisementTypes { get; set; }
        public virtual DbSet<AdvertisementFiles> AdvertisementFiles { get; set; }
        public virtual DbSet<AdvertisementFeaturesValues> AdvertisementFeaturesValues { get; set; }
        public virtual DbSet<AdvertisementPricesHistories> AdvertisementPricesHistories { get; set; }
        public virtual DbSet<AdvertisementAddress> AdvertisementAddress { get; set; }
        public virtual DbSet<AdvertisementDetails> AdvertisementDetails { get; set; }
        public virtual DbSet<Banners> Banners { get; set; }
        public virtual DbSet<AdvertisementFeatures> AdvertisementFeatures { get; set; }
        public virtual DbSet<AdvertisementOwners> AdvertisementOwners { get; set; }
        public virtual DbSet<AdvertisementViewers> AdvertisementViewers { get; set; }
        public virtual DbSet<AdvertisementCallers> AdvertisementCallers { get; set; }
        public virtual DbSet<AdvertisementFavorites> AdvertisementFavorites { get; set; }
        public virtual DbSet<BuildingLifes> BuildingLifes { get; set; }
        public virtual DbSet<AdvertisementDataTypeCounts> AdvertisementDataTypeCounts { get; set; }
        public virtual DbSet<AdvertisementAdvanceSearchVM> AdvertisementAdvanceSearchVM { get; set; }
        public virtual DbSet<AdvertisementStatusListVM> AdvertisementStatusListVM { get; set; }
        public virtual DbSet<AdvertisementSelectedCallers> AdvertisementSelectedCallers { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<StatusTypes> StatusTypes { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MelkavanConnection"));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("MelkavanConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Melkavan

            modelBuilder.Entity<Banners>(entity =>
            {
                entity.HasKey(e => e.BannerId);
            });

            modelBuilder.Entity<AdvertisementDataTypeCounts>(entity =>
            {
                entity.HasKey(e => e.AdvertisementDataTypeCountId);
            });

            modelBuilder.Entity<AdvertisementAdvanceSearchVM>(entity =>
            {
                entity.HasKey(e => e.AdvertisementId);
            });

            modelBuilder.Entity<AdvertisementStatusListVM>(entity =>
            {
                entity.HasKey(e => e.AdvertisementId);
            });

            #endregion
        }

    }
}
