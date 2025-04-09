using APIs.Teniaco.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Teniaco;
using VM.Teniaco.VM.Teniaco;
using Properties = APIs.Teniaco.Models.Entities.Properties;

namespace APIs.Teniaco.Models
{
    public partial class TeniacoApiContext : DbContext
    {
        public TeniacoApiContext()
        {
        }

        public TeniacoApiContext(DbContextOptions<TeniacoApiContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(int.MaxValue);
        }

        #region Teniaco

        public virtual DbSet<ElementTypes> ElementTypes { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<FeaturesCategories> FeaturesCategories { get; set; }
        public virtual DbSet<FeaturesOptions> FeaturesOptions { get; set; }
        public virtual DbSet<FeaturesValues> FeaturesValues { get; set; }
        //public virtual DbSet<IntroductionMethods> IntroductionMethods { get; set; }
        public virtual DbSet<Investors> Investors { get; set; }
        public virtual DbSet<InvestorsFavorites> InvestorsFavorites { get; set; }
        public virtual DbSet<InvestorsRequests> InvestorsRequests { get; set; }
        //public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectStates> ProjectStates { get; set; }
        //public virtual DbSet<ProjectTypes> ProjectTypes { get; set; }
        public virtual DbSet<Properties> Properties { get; set; }
        public virtual DbSet<PropertiesPricesHistories> PropertiesPricesHistories { get; set; }
        public virtual DbSet<PropertyAddress> PropertyAddress { get; set; }
        public virtual DbSet<PropertyFiles> PropertyFiles { get; set; }
        public virtual DbSet<PropertyStates> PropertyStates { get; set; }
        public virtual DbSet<PropertyTypes> PropertyTypes { get; set; }
        public virtual DbSet<ProposedProjects> ProposedProjects { get; set; }
        public virtual DbSet<TypeOfUses> TypeOfUses { get; set; }
        public virtual DbSet<DocumentOwnershipTypes> DocumentOwnershipTypes { get; set; }
        public virtual DbSet<DocumentRootTypes> DocumentRootTypes { get; set; }
        public virtual DbSet<DocumentTypes> DocumentTypes { get; set; }
        public virtual DbSet<Agencies> Agencies { get; set; }
        public virtual DbSet<AgencyStaffs> AgencyStaffs { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Funds> Funds { get; set; }
        public virtual DbSet<Contractors> Contractors { get; set; }
        public virtual DbSet<MapLayerCategories> MapLayerCategories { get; set; }
        public virtual DbSet<MapLayers> MapLayers { get; set; }
        public virtual DbSet<EvaluationCategories> EvaluationCategories { get; set; }
        public virtual DbSet<EvaluationQuestions> EvaluationQuestions { get; set; }
        public virtual DbSet<EvaluationItems> EvaluationItems { get; set; }
        public virtual DbSet<EvaluationItemValues> EvaluationItemValues { get; set; }
        public virtual DbSet<Evaluations> Evaluations { get; set; }
        public virtual DbSet<EvaluationSubjects> EvaluationSubjects { get; set; }
        public virtual DbSet<PropertyOwners> PropertyOwners { get; set; }
        public virtual DbSet<PropertiesViewers> PropertiesViewers { get; set; }
        public virtual DbSet<PropertiesCallers> PropertiesCallers { get; set; }
        public virtual DbSet<PropertiesFavorites> PropertiesFavorites { get; set; }
        public virtual DbSet<PropertiesDetails> PropertiesDetails { get; set; }

        public virtual DbSet<PropertySelectedCallers> PropertySelectedCallers { get; set; }

        public virtual DbSet<PropertyDataTypeCounts> PropertyDataTypeCounts { get; set; }

        public virtual DbSet<PropertiesPricesForMapVM> PropertiesPricesForMapVM { get; set; }

        public virtual DbSet<PropertiesAdvanceSearchVM> PropertiesAdvanceSearchVM { get; set; }

        public virtual DbSet<OutterDashboardPricesVM> OutterDashboardPricesVM { get; set; }

        //پراکندگی سرمایه
        public virtual DbSet<MyFundsDispersionVM> MyFundsDispersionVM { get; set; }

        //رشد سرمایه
        public virtual DbSet<MyFundsGrowthVM> MyFundsGrowthVM { get; set; }
        //خریداران
        public virtual DbSet<PropertyBuyers> PropertyBuyers { get; set; }
        // املاک من
        public virtual DbSet<MyPropertiesForInvestorsAdvanceSearchVM> MyPropertiesForInvestorsAdvanceSearchVM { get; set; }



        #region Map To VM
        [DbFunction("Udf_InvestorWithFullNameList", Schema = "dbo")]
        public IQueryable<InvestorWithFullName> GetInvestorWithFullName(int InvestorId)
                => FromExpression(() => GetInvestorWithFullName(InvestorId));
        public IQueryable<InvestorWithFullName> InvestorWithFullNames { get; set; }
        #endregion


        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("TeniacoConnection"));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("TeniacoConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Teniaco

            modelBuilder.Entity<ElementTypes>()
                .HasMany(e => e.Features)
                .WithOne(e => e.ElementTypes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Features>()
                .HasMany(e => e.FeaturesValues)
                .WithOne(e => e.Features)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<EvaluationQuestions>()
            //    .Property(t => t.RootId).ValueGeneratedOnAdd()
            //    .HasComputedColumnSql("[dbo].[udf_GetRootEvalCat]([EvaluationCategoryId])");

            //modelBuilder.Entity<IntroductionMethods>()
            //    .HasMany(e => e.Investors)
            //    .WithOne(e => e.IntroductionMethods);

            //modelBuilder.Entity<Investors>()
            //    .HasMany(e => e.InvestorsFavorites)
            //    .WithOne(e => e.Investors)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Investors>()
            //    .HasMany(e => e.InvestorsRequests)
            //    .WithOne(e => e.Investors)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Investors>()
            //    .HasMany(e => e.ProposedProjects)
            //    .WithOne(e => e.Investors)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Projects>()
            //    .Property(e => e.ProjectTitle)
            //    .IsFixedLength();

            //modelBuilder.Entity<Projects>()
            //    .HasMany(e => e.InvestorsRequests)
            //    .WithOne(e => e.Projects)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Projects>()
            //    .HasMany(e => e.Projects1)
            //    .WithOne(e => e.Projects2)
            //    .HasForeignKey(e => e.ParentProjectId);

            //modelBuilder.Entity<Projects>()
            //    .HasMany(e => e.ProposedProjects)
            //    .WithOne(e => e.Projects)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<ProjectTypes>()
            //    .HasMany(e => e.Projects)
            //    .WithOne(e => e.ProjectTypes)
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Properties>()
                .HasOne(e => e.InvestorsFavorites)
                .WithOne(e => e.Properties);

            //modelBuilder.Entity<Properties>()
            //    .HasMany(e => e.Projects)
            //    .WithOne(e => e.Properties)
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Properties>()
                .HasOne(e => e.PropertyAddress)
                .WithOne(e => e.Properties);

            modelBuilder.Entity<Properties>()
                .HasMany(e => e.PropertyFiles)
                .WithOne(e => e.Properties)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Properties>()
                .HasMany(e => e.PropertyOwners)
                .WithOne(e => e.Properties)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<PropertyStates>()
            //    .HasMany(e => e.Properties)
            //    .WithOne(e => e.PropertyStates)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<PropertyTypes>()
            //    .HasMany(e => e.FeaturesValues)
            //    .WithOne(e => e.PropertyTypes)
            //    .HasForeignKey(e => e.PropertyId);

            modelBuilder.Entity<PropertyTypes>()
                .HasMany(e => e.Properties)
                .WithOne(e => e.PropertyTypes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TypeOfUses>()
                .HasMany(e => e.Properties)
                .WithOne(e => e.TypeOfUses)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentOwnershipTypes>()
                .HasMany(e => e.Properties)
                .WithOne(e => e.DocumentOwnershipTypes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentRootTypes>()
                .HasMany(e => e.Properties)
                .WithOne(e => e.DocumentRootTypes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentTypes>()
                .HasMany(e => e.Properties)
                .WithOne(e => e.DocumentTypes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PropertiesPricesForMapVM>(entity =>
            {
                entity.HasKey(e => e.PropertyId);
            });

            modelBuilder.Entity<PropertyDataTypeCounts>(entity =>
            {
                entity.HasKey(e => e.PropertyDataTypeCountId);
            });

            modelBuilder.Entity<PropertiesAdvanceSearchVM>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.RecordType,
                    e.AdvertisementId
                });
            });

            modelBuilder.Entity<MyPropertiesForInvestorsAdvanceSearchVM>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.RecordType,
                    e.PropertyId
                });
            });

            modelBuilder.Entity<OutterDashboardPricesVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            modelBuilder.Entity<MyFundsDispersionVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            modelBuilder.Entity<MyFundsGrowthVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            //modelBuilder.Entity<PropertiesAdvanceSearchVM>(entity =>
            //{
            //    entity.HasNoKey();
            //    //entity.HasKey(e => new
            //    //{
            //    //    e.RecordType,
            //    //    e.AdvertisementId
            //    //});
            //});

            //modelBuilder.Entity<PropertiesAdvanceSearch>(entity =>
            //{
            //    entity.HasNoKey();
            //    //entity.HasKey(e => new
            //    //{
            //    //    e.RecordType,
            //    //    e.AdvertisementId
            //    //});
            //});

            #endregion
        }
    }
}
