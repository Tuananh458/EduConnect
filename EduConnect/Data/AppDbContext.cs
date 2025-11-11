using Microsoft.EntityFrameworkCore;
using EduConnect.Models.School;
using EduConnect.Models.HocLieu;
using EduConnect.Models;

namespace EduConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ==========================
        // 🧩 MODULE 1: HỌC LIỆU
        // ==========================
        public DbSet<HocLieu> HocLieus { get; set; }
        public DbSet<CauHoiHocLieu> CauHoiHocLieus { get; set; }
        public DbSet<HocLieuCauHoi> HocLieuCauHois { get; set; }
        public DbSet<BaiLamHocLieu> BaiLamHocLieus { get; set; }
        public DbSet<BaiLamChiTiet> BaiLamChiTiets { get; set; }

        // ==========================
        // 🧩 MODULE 2: XÁC THỰC / TÀI KHOẢN
        // ==========================
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<TokenLamMoi> TokenLamMois { get; set; }
        public DbSet<DatLaiMatKhau> DatLaiMatKhaus { get; set; }
        public DbSet<XacThucEmail> XacThucEmails { get; set; }


        // ==========================
        // 🧩 MODULE 3: TRƯỜNG HỌC
        // ==========================
        public DbSet<KhoiHoc> KhoiHocs { get; set; }
        public DbSet<LopHoc> LopHocs { get; set; }
        public DbSet<HocSinh> HocSinhs { get; set; }
        public DbSet<GiaoVien> GiaoViens { get; set; }
        public DbSet<PhuHuynh> PhuHuynhs { get; set; }
        public DbSet<LienKetPhuHuynhHocSinh> LienKetPhuHuynhHocSinhs { get; set; }

        // ==========================
        // ⚙️ CẤU HÌNH QUAN HỆ
        // ==========================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================
            // 🔐 NGUOIDUNG - TOKEN
            // =====================================
            modelBuilder.Entity<NguoiDung>()
                .HasMany(u => u.TokenLamMois)
                .WithOne(r => r.NguoiDung)
                .HasForeignKey(r => r.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(u => u.TenDangNhap).IsUnique();

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<NguoiDung>()
                .Property(u => u.AnhDaiDien)
                .HasMaxLength(512)
                .HasDefaultValue("/template/img/avt.svg");

            // =====================================
            // 🧩 QUAN HỆ TRƯỜNG HỌC
            // =====================================

            // ⚙️ Học sinh - Người dùng
            modelBuilder.Entity<HocSinh>()
                .HasOne(h => h.NguoiDung)
                .WithMany()
                .HasForeignKey(h => h.MaNguoiDung)
                .OnDelete(DeleteBehavior.NoAction);

            // ⚙️ Giáo viên - Người dùng
            modelBuilder.Entity<GiaoVien>()
                .HasOne(g => g.NguoiDung)
                .WithMany()
                .HasForeignKey(g => g.MaNguoiDung)
                .OnDelete(DeleteBehavior.NoAction);

            // ⚙️ Phụ huynh - Người dùng
            modelBuilder.Entity<PhuHuynh>()
                .HasOne(p => p.NguoiDung)
                .WithMany()
                .HasForeignKey(p => p.MaNguoiDung)
                .OnDelete(DeleteBehavior.NoAction);

       
            // ⚙️ Lớp học - Khối học
            modelBuilder.Entity<LopHoc>()
                .HasOne(l => l.KhoiHoc)
                .WithMany()
                .HasForeignKey(l => l.MaKhoiHoc)
                .OnDelete(DeleteBehavior.Restrict);
            // ⚙️ Lớp học - Người tạo (Người dùng)
            modelBuilder.Entity<LopHoc>()
                 .HasOne(l => l.NguoiTao)
                 .WithMany()
                 .HasForeignKey(l => l.NguoiTaoId)
                 .OnDelete(DeleteBehavior.Restrict);
            // ⚙️ Quan hệ 1 giáo viên - nhiều lớp chủ nhiệm
            modelBuilder.Entity<LopHoc>()
                .HasOne(l => l.GiaoVienChuNhiem)
                .WithMany(gv => gv.LopChuNhiems)
                .HasForeignKey(l => l.MaGiaoVienChuNhiem)
                .OnDelete(DeleteBehavior.SetNull);


            // ⚙️ Học sinh - Lớp học
            modelBuilder.Entity<HocSinh>()
                .HasOne(h => h.LopHoc)
                .WithMany()
                .HasForeignKey(h => h.MaLopHoc)
                .OnDelete(DeleteBehavior.Cascade);

            // ⚙️ Liên kết Phụ huynh - Học sinh
            modelBuilder.Entity<LienKetPhuHuynhHocSinh>()
                .HasOne(lk => lk.PhuHuynh)
                .WithMany(p => p.LienKetPhuHuynhHocSinhs)
                .HasForeignKey(lk => lk.MaPhuHuynh)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LienKetPhuHuynhHocSinh>()
                .HasOne(lk => lk.HocSinh)
                .WithMany()
                .HasForeignKey(lk => lk.MaHocSinh)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================================
            // 🧩 QUAN HỆ HỌC LIỆU
            // =====================================
            modelBuilder.Entity<CauHoiHocLieu>()
                .HasOne(ch => ch.HocLieu)
                .WithMany(hl => hl.CauHois)
                .HasForeignKey(ch => ch.HocLieuId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BaiLamHocLieu>()
                .HasOne(b => b.HocLieu)
                .WithMany(hl => hl.BaiLams)
                .HasForeignKey(b => b.HocLieuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BaiLamHocLieu>()
                .HasOne(b => b.HocSinh)
                .WithMany()
                .HasForeignKey(b => b.HocSinhId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BaiLamChiTiet>()
                .HasOne(ct => ct.BaiLamHocLieu)
                .WithMany(b => b.ChiTiets)
                .HasForeignKey(ct => ct.MaBaiLam)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HocLieuCauHoi>()
                .HasOne(hch => hch.HocLieu)
                .WithMany(hl => hl.HocLieuCauHois)
                .HasForeignKey(hch => hch.HocLieuId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HocLieuCauHoi>()
                .HasOne(hch => hch.CauHoi)
                .WithMany()
                .HasForeignKey(hch => hch.CauHoiId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================================
            // 🌱 SEED DỮ LIỆU KHỐI HỌC
            // =====================================
            modelBuilder.Entity<KhoiHoc>().HasData(
                new KhoiHoc { MaKhoiHoc = 10, TenKhoiHoc = "Khối 10" },
                new KhoiHoc { MaKhoiHoc = 11, TenKhoiHoc = "Khối 11" },
                new KhoiHoc { MaKhoiHoc = 12, TenKhoiHoc = "Khối 12" }
            );
        }
    }
}
