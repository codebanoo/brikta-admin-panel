using APIs.Projects.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Projects;
using VM.Teniaco;

namespace APIs.Projects.Models
{
    public partial class ProjectsApiContext : DbContext
    {
        public ProjectsApiContext()
        {
        }

        public ProjectsApiContext(DbContextOptions<ProjectsApiContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(int.MaxValue);
        }

        #region Projects

        public virtual DbSet<PropertyProjects> PropertyProjects { get; set; }
        public virtual DbSet<PropertyProjectTypes> ProjectTypes { get; set; }
        public virtual DbSet<OperationsOnProperty> OperationsOnProperty { get; set; }
        public virtual DbSet<OperationOnPropertyProjects> OperationOnPropertyProjects { get; set; }
        public virtual DbSet<ConstructionProjects> ConstructionProjects { get; set; }
        public virtual DbSet<PartnershipAgreements> PartnershipAgreements { get; set; }
        public virtual DbSet<ContractorsAgreements> ContractorsAgreements { get; set; }
        public virtual DbSet<ContractAgreements> ContractAgreements { get; set; }
        public virtual DbSet<ConfirmationTypes> ConfirmationTypes { get; set; }
        public virtual DbSet<ConfirmationAgreements> ConfirmationAgreements { get; set; }
        public virtual DbSet<AttachementFiles> AttachementFiles { get; set; }
        public virtual DbSet<ContractSides> ContractSides { get; set; }
        public virtual DbSet<InitialPlans> InitialPlans { get; set; }
        public virtual DbSet<PitchDecks> PitchDecks { get; set; }
        public virtual DbSet<MeetingBoards> MeetingBoards { get; set; }
        public virtual DbSet<ProgressPictures> ProgressPictures { get; set; }
        public virtual DbSet<TimeTables> TimeTables { get; set; }
        public virtual DbSet<TimeTableItems> TimeTableItems { get; set; }
        public virtual DbSet<GoogleSheetConfigs> GoogleSheetConfigs { get; set; }
        public virtual DbSet<ExcelSheetConfigs> ExcelSheetConfigs { get; set; }
        public virtual DbSet<ExcelSheetConfigHistories> ExcelSheetConfigHistories { get; set; }
        public virtual DbSet<ConstructionProjectDailyTitles> ConstructionProjectDailyTitles { get; set; }
        public virtual DbSet<ConstructionProjectDailyData> ConstructionProjectDailyData { get; set; }
        public virtual DbSet<ConstructionProjectFinancialTitles> ConstructionProjectFinancialTitles { get; set; }
        public virtual DbSet<ConstructionProjectFinancialData> ConstructionProjectFinancialData { get; set; }
        public virtual DbSet<ConstructionProjectOwnerUsers> ConstructionProjectOwnerUsers { get; set; }
        public virtual DbSet<ConstructionProjectRepresentatives> ConstructionProjectRepresentatives { get; set; }
        public virtual DbSet<ConstructionProjectTypes> ConstructionProjectTypes { get; set; }
        public virtual DbSet<ConstructionProjectPriceHistories> ConstructionProjectPriceHistories { get; set; }
        public virtual DbSet<FileStatesLogs> FileStatesLogs { get; set; }
        public virtual DbSet<FileStates> FileStates { get; set; }
        public virtual DbSet<ConversationLogs> ConversationLogs { get; set; }
        public virtual DbSet<ConstructionProjectDailyPictures> ConstructionProjectDailyPictures { get; set; }

        public virtual DbSet<ConstructionProjectDataTypeCounts> ConstructionProjectDataTypeCounts { get; set; }

        public virtual DbSet<OutterDashboardPricesVM> OutterDashboardPricesVM { get; set; }

        public virtual DbSet<MyFundsDispersionVM> MyFundsDispersionVM { get; set; }

        public virtual DbSet<MyFundsGrowthVM> MyFundsGrowthVM { get; set; }

        public virtual DbSet<PaymentsAndCostsDataVM> PaymentsAndCostsDataVM { get; set; }

        public virtual DbSet<SumOfPrivateCostsVM> SumOfPrivateCostsVM { get; set; }
        public virtual DbSet<SumOfPrivateCostsForRepresantativeVM> SumOfPrivateCostsForRepresantativeVM { get; set; }
        public virtual DbSet<SumOfPublicCostsVM> SumOfPublicCostsVM { get; set; }

        public virtual DbSet<SumOfPublicCostsRepresentativesVM> SumOfPublicCostsRepresentativesVM { get; set; }

        public virtual DbSet<ConstructionProjectProgressData> ConstructionProjectProgressData { get; set; }

        public virtual DbSet<ConstructionProjectProgressItems> ConstructionProjectProgressItems { get; set; }
        public virtual DbSet<ConstructionProjectDelays> ConstructionProjectDelays { get; set; }
        public virtual DbSet<ConstructionProjectBillDelays> ConstructionProjectBillDelays { get; set; }
        public virtual DbSet<TeniacoSuggestionFiles> TeniacoSuggestionFiles { get; set; }

        public virtual DbSet<ConversationLogsForOuterDashboardVM> ConversationLogsForOuterDashboard { get; set; }
        public virtual DbSet<FileStatesLogsForOuterDashboardVM> FileStatesLogsForOuterDashboard { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ProjectsConnection"));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("ProjectsConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Projects

            modelBuilder.Entity<PropertyProjects>(entity =>
            {
                entity.HasKey(e => e.PropertyProjectId);
            });


            modelBuilder.Entity<OperationsOnProperty>(entity =>
            {
                entity.HasKey(e => e.OperationOnPropertyId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<OperationOnPropertyProjects>(entity =>
            {
                entity.HasKey(e => e.OperationOnPropertyProjectId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<PropertyProjectTypes>(entity =>
            {
                entity.HasKey(e => e.PropertyProjectTypeId);

                //entity.Property(e => e.CreateTime).HasMaxLength(10);

                //entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<ConstructionProjectDataTypeCounts>(entity =>
            {
                entity.HasKey(e => e.ConstructionProjectDataTypeCountId);
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

            modelBuilder.Entity<PaymentsAndCostsDataVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            modelBuilder.Entity<SumOfPrivateCostsVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            modelBuilder.Entity<SumOfPrivateCostsForRepresantativeVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            modelBuilder.Entity<SumOfPublicCostsVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });


            modelBuilder.Entity<SumOfPublicCostsRepresentativesVM>(entity =>
            {
                entity.HasKey(e => e.RowNumber);
            });

            modelBuilder.Entity < ConstructionProjectProgressData>(entity =>
            {
                entity.HasKey(e => e.ConstructionProjectProgressDataId);
            });

            modelBuilder.Entity<ConstructionProjectProgressItems>(entity =>
            {
                entity.HasKey(e => e.ConstructionProjectProgressItemId);
            });


            modelBuilder.Entity<ConstructionProjectRepresentatives>(entity =>
            {
                entity.HasKey(e => e.ConstructionProjectRepresentativeId);
            });

            modelBuilder.Entity<ConversationLogsForOuterDashboardVM>(entity =>
            {
                entity.HasKey(e => e.ConversationLogId);
            });

            modelBuilder.Entity<FileStatesLogsForOuterDashboardVM>(entity =>
            {
                entity.HasKey(e => e.FileStateLogId);
            });

            #endregion
        }

    }
}
