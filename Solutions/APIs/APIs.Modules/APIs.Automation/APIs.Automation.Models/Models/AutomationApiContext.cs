using APIs.Automation.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIs.Automation.Models
{
    public partial class AutomationApiContext : DbContext
    {
        public AutomationApiContext()
        {
        }

        public AutomationApiContext(DbContextOptions<AutomationApiContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(int.MaxValue);
        }

        #region Automation

        public virtual DbSet<OrgChartNodeIds> OrgChartNodeIds { get; set; }
        public virtual DbSet<MyCompanies> MyCompanies { get; set; }
        public virtual DbSet<MyCompaniesDirectors> MyCompaniesDirectors { get; set; }
        public virtual DbSet<MyDepartments> MyDepartments { get; set; }
        public virtual DbSet<MyDepartmentsDirectors> MyDepartmentsDirectors { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<DepartmentsStaff> DepartmentsStaff { get; set; }
        public virtual DbSet<NodeTypes> NodeTypes { get; set; }
        public virtual DbSet<OrgChartNodes> OrgChartNodes { get; set; }
        public virtual DbSet<BoardMembers> BoardMembers { get; set; }
        public virtual DbSet<Forms> Forms { get; set; }
        public virtual DbSet<FormElements> FormElements { get; set; }
        public virtual DbSet<FormElementOptions> FormElementOptions { get; set; }
        public virtual DbSet<FormElementValues> FormElementValues { get; set; }

        public virtual DbSet<OrganizationalPositions> OrganizationalPositions { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("AutomationConnection"));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("AutomationConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Automation

            modelBuilder.Entity<MyCompanies>(entity =>
            {
                entity.HasKey(e => e.MyCompanyId);

                //entity.Property(e => e.MyCompanyDirectorId).ValueGeneratedNever();

                entity.HasIndex(e => e.UserIdCreator);

                entity.HasIndex(e => e.UserIdEditor);

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<MyCompaniesDirectors>(entity =>
            {
                entity.HasKey(e => e.MyCompanyDirectorId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<MyDepartments>(entity =>
            {
                entity.HasKey(e => e.MyDepartmentId);

                //entity.HasIndex(e => e.MyCompanyId);

                entity.HasIndex(e => e.UserIdCreator);

                entity.HasIndex(e => e.UserIdEditor);

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);

                //entity.HasOne(d => d.MyCompanies)
                //    .WithMany(p => p.MyDepartments)
                //    .HasForeignKey(d => d.MyCompanyId)
                //    .OnDelete(DeleteBehavior.Cascade);

                //entity.HasOne(d => d.MyDepartmentsDirectors)
                //    .WithMany(p => p.MyDepartments)
                //    .HasForeignKey(d => d.MyDepartmentDirectorId)
                //    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MyDepartmentsDirectors>(entity =>
            {
                entity.HasKey(e => e.MyDepartmentDirectorId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<DepartmentsStaff>(entity =>
            {
                entity.HasKey(e => e.DepartmentStaffId);

                entity.HasIndex(e => e.UserId);

                entity.HasIndex(e => e.MyDepartmentId);

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<NodeTypes>(entity =>
            {
                entity.HasKey(e => e.NodeTypeId);
            });

            modelBuilder.Entity<OrgChartNodes>(entity =>
            {
                entity.HasKey(e => e.OrgChartNodeId);

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<BoardMembers>(entity =>
            {
                entity.HasKey(e => e.BoardMemberId);

                entity.Property(e => e.CreateTime).HasMaxLength(10);

                entity.Property(e => e.EditTime).HasMaxLength(10);
            });

            modelBuilder.Entity<OrgChartNodeIds>(entity =>
            {
                entity.HasKey(e => e.OrgChartNodeId);
            });

            #endregion
        }
    }
}
