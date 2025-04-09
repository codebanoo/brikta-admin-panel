using APIs.Public.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIs.Public.Models
{
    public partial class PublicApiContext : DbContext
    {
        public PublicApiContext()
        {
        }

        public PublicApiContext(DbContextOptions<PublicApiContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(int.MaxValue);
        }

        #region Public

        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Zones> Zones { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Persons> Persons { get; set; }
        public virtual DbSet<PersonTypes> PersonTypes { get; set; }
        public virtual DbSet<FormUsages> FormUsages { get; set; }
        public virtual DbSet<ElementTypes> ElementTypes { get; set; }
        public virtual DbSet<TypeOfUseLayers> TypeOfUseLayers { get; set; }
        public virtual DbSet<GuildCategories> GuildCategories { get; set; }
        public virtual DbSet<DistrictFiles> DistrictFiles { get; set; }
        public virtual DbSet<ZoneFiles> ZoneFiles { get; set; }
        public virtual DbSet<Indices> Indices { get; set; }
        public virtual DbSet<PricesListIndices> PricesListIndices { get; set; }
        public virtual DbSet<ConstructionItems> ConstructionItems { get; set; }
        public virtual DbSet<ConstructionSubItems> ConstructionSubItems { get; set; }

        public virtual DbSet<ConstructionItemPricesHistories> ConstructionItemPricesHistories { get; set; }
        public virtual DbSet<UnitsOfMeasurement> UnitsOfMeasurement { get; set; }
        public virtual DbSet<SmsSenders> SmsSenders { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PublicConnection"));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("PublicConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Public

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryId);
            });


            modelBuilder.Entity<States>(entity =>
            {
                entity.HasKey(e => e.StateId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(e => e.CityId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<Zones>(entity =>
            {
                entity.HasKey(e => e.ZoneId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });
            modelBuilder.Entity<Districts>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

            });

            modelBuilder.Entity<Persons>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<PersonTypes>(entity =>
            {
                entity.HasKey(e => e.PersonTypeId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<FormUsages>(entity =>
            {
                entity.HasKey(e => e.FormUsageId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<ElementTypes>(entity =>
            {
                entity.HasKey(e => e.ElementTypeId);
            });

            modelBuilder.Entity<TypeOfUseLayers>(entity =>
            {
                entity.HasKey(e => e.TypeOfUseLayersId);
            });

            modelBuilder.Entity<GuildCategories>(entity =>
            {
                entity.HasKey(e => e.GuildCategoryId);
            });

            modelBuilder.Entity<Indices>(entity =>
            {
                entity.HasKey(e => e.IndiceId);
            });

            modelBuilder.Entity<PricesListIndices>(entity =>
            {
                entity.HasKey(e => e.PricesListIndicesId);
            });

            modelBuilder.Entity<ConstructionItems>(entity =>
            {
                entity.HasKey(e => e.ConstructionItemId);
            });

            modelBuilder.Entity<ConstructionSubItems>(entity =>
            {
                entity.HasKey(e => e.ConstructionSubItemId);
            });

            modelBuilder.Entity<ConstructionItemPricesHistories>(entity =>
            {
                entity.HasKey(e => e.ConstructionItemPricesHistoryId);
            });

            modelBuilder.Entity<UnitsOfMeasurement>(entity =>
            {
                entity.HasKey(e => e.UnitOfMeasurementId);
            });

            modelBuilder.Entity<PricesListIndices>()
              .Property(t => t.Change).ValueGeneratedOnAdd()
              .HasComputedColumnSql("[dbo].[udf_ChangePriceIndex]([PricesListIndicesId])");

            #endregion
        }

    }
}
