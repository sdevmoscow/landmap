using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LandMap.Models;

namespace LandMap.Data
{
    public partial class LandDbContext : DbContext
    {
        public LandDbContext()
        {
        }

        public LandDbContext(DbContextOptions<LandDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Land> Land { get; set; }
        public virtual DbSet<LandRightType> LandRightType { get; set; }
        public virtual DbSet<LandType> LandType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=ms-sql-9.in-solve.ru;Database=1gb_academics;Trusted_Connection=False;User Id=1gb_elizarovsa;Password=za5d8f88aa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Land>(entity =>
            {
                entity.Property(e => e.CadastralNumber).HasMaxLength(30);

                entity.Property(e => e.Coordinates).IsUnicode(false);

                entity.Property(e => e.InventoryNumber).HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.LandRightType)
                    .WithMany(p => p.Land)
                    .HasForeignKey(d => d.LandRightTypeId)
                    .HasConstraintName("FK_Land_LandRightType");

                entity.HasOne(d => d.LandType)
                    .WithMany(p => p.Land)
                    .HasForeignKey(d => d.LandTypeId)
                    .HasConstraintName("FK_Land_LandType");
            });

            modelBuilder.Entity<LandRightType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LandType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }

        public DbSet<LandMap.Models.Land> Land_1 { get; set; }
    }
}
