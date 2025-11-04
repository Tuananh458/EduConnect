using Microsoft.EntityFrameworkCore;
using EduConnect.Models.School;
using EduConnect.Models.Auth;
using EduConnect.Models;
using EduConnect.Models.HocLieu;
using EduConnect.Models.DanhMuc;

namespace EduConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ==========================
        // 🧩 MODULE 1: Học liệu
        // ==========================



        public DbSet<EduConnect.Models.HocLieu.HocLieu> HocLieus { get; set; }
        public DbSet<CauHoiHocLieu> CauHoiHocLieus => Set<CauHoiHocLieu>();

        // ==========================
        // 🧩 MODULE 2: Xác thực / Tài khoản
        // ==========================
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
   

        // ==========================
        // 🧩 MODULE 3: Trường học
        // ==========================

        public DbSet<KhoiHoc> KhoiHocs { get; set; }
        public DbSet<LopHoc> LopHocs { get; set; }
        public DbSet<HocSinh> HocSinhs { get; set; }
        public DbSet<GiaoVien> GiaoViens { get; set; }
        public DbSet<PhuHuynh> PhuHuynhs { get; set; }
        public DbSet<LienKetPhuHuynhHocSinh> LienKetPhuHuynhHocSinhs { get; set; }

        // ==========================
        // ⚙️ Cấu hình quan hệ & seed data
        // ==========================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----- User <-> RefreshToken (1-n)
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔒 Unique constraint cho Auth
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            // ----- KhoiHoc <-> LopHoc (1-n)
            modelBuilder.Entity<LopHoc>()
                .HasOne(l => l.KhoiHoc)
                .WithMany()
                .HasForeignKey(l => l.MaKhoiHoc)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- PhuHuynh <-> HocSinh (n-n)
            modelBuilder.Entity<LienKetPhuHuynhHocSinh>()
                .HasOne(l => l.PhuHuynh)
                .WithMany(p => p.LienKetPhuHuynhHocSinhs)
                .HasForeignKey(l => l.MaPhuHuynh)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LienKetPhuHuynhHocSinh>()
                .HasOne(l => l.HocSinh)
                .WithMany(h => h.LienKetPhuHuynhHocSinhs)
                .HasForeignKey(l => l.MaHocSinh)
                .OnDelete(DeleteBehavior.Cascade);
            // Quan hệ HocLieu - CauHoiHocLieu: 1-n
            modelBuilder.Entity<CauHoiHocLieu>()
                .HasOne(ch => ch.HocLieu)
                .WithMany(hl => hl.CauHois)
                .HasForeignKey(ch => ch.HocLieuId)
                .OnDelete(DeleteBehavior.Cascade);
            // ==========================
            // 🌱 SEED DỮ LIỆU KHỐI HỌC MẶC ĐỊNH
            // ==========================
            modelBuilder.Entity<KhoiHoc>().HasData(
                new KhoiHoc { MaKhoiHoc = 10, TenKhoiHoc = "Khối 10" },
                new KhoiHoc { MaKhoiHoc = 11, TenKhoiHoc = "Khối 11" },
                new KhoiHoc { MaKhoiHoc = 12, TenKhoiHoc = "Khối 12" }
            );
        }
    }
}
