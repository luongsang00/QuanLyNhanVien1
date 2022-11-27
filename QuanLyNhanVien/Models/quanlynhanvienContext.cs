using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuanLyNhanVien.Models
{
    public partial class quanlynhanvienContext : DbContext
    {
        public quanlynhanvienContext()
        {
        }

        public quanlynhanvienContext(DbContextOptions<quanlynhanvienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietKyNang> ChiTietKyNangs { get; set; } = null!;
        public virtual DbSet<KyNang> KyNangs { get; set; } = null!;
        public virtual DbSet<LoaiNhanVien> LoaiNhanViens { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=quanlynhanvien;userid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.27-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<ChiTietKyNang>(entity =>
            {
                entity.HasKey(e => new { e.IdNhanVien, e.IdKyNang })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("chi_tiet_ky_nang");

                entity.Property(e => e.IdNhanVien)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_NhanVien");

                entity.Property(e => e.IdKyNang)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_KyNang");
            });

            modelBuilder.Entity<KyNang>(entity =>
            {
                entity.HasKey(e => e.IdKyNang)
                    .HasName("PRIMARY");

                entity.ToTable("ky_nang");

                entity.Property(e => e.IdKyNang)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_Ky_Nang");

                entity.Property(e => e.TenLoaiKn)
                    .HasMaxLength(255)
                    .HasColumnName("Ten_LoaiKN");
            });

            modelBuilder.Entity<LoaiNhanVien>(entity =>
            {
                entity.HasKey(e => e.IdLoaiNv)
                    .HasName("PRIMARY");

                entity.ToTable("loai_nhan_vien");

                entity.Property(e => e.IdLoaiNv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_LoaiNV");

                entity.Property(e => e.TenLoaiNv)
                    .HasMaxLength(255)
                    .HasColumnName("Ten_LoaiNV");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.ToTable("nhan_vien");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DiaChi)
                    .HasMaxLength(255)
                    .HasColumnName("Dia_Chi");

                entity.Property(e => e.IdLoaiNv)
                    .HasColumnType("int(255)")
                    .HasColumnName("Id_LoaiNv");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(255)
                    .HasColumnName("So_Dien_Thoai");

                entity.Property(e => e.Ten).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
