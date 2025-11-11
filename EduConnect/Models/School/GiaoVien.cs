using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduConnect.Models.School;

namespace EduConnect.Models
{
    [Table("GIAOVIEN")]
    public class GiaoVien
    {
        [Key]
        [Column("maGiaoVien")]
        public int MaGiaoVien { get; set; }

        // 🔹 Liên kết với người dùng
        [Required]
        [Column("maNguoiDung")]
        public Guid MaNguoiDung { get; set; }

        [ForeignKey(nameof(MaNguoiDung))]
        public NguoiDung NguoiDung { get; set; } = default!;

        // 🔹 Thông tin chuyên môn
        [Column("chuyenMon"), MaxLength(100)]
        public string? ChuyenMon { get; set; }

        [Column("trinhDo"), MaxLength(60)]
        public string? TrinhDo { get; set; }

        // 🔹 Ngày bắt đầu công tác
        [Column("ngayBatDau")]
        public DateTime? NgayBatDau { get; set; }

        // 🔹 Địa chỉ nơi ở hoặc công tác
        [Column("diaChi"), MaxLength(255)]
        public string? DiaChi { get; set; }

        // 🔹 Trạng thái công tác (đang dạy hay nghỉ)
        [Column("trangThaiCongTac")]
        public bool TrangThaiCongTac { get; set; } = true;

        // 🔹 Ngày tạo hồ sơ
        [Column("ngayTao")]
        public DateTime? NgayTao { get; set; } = DateTime.UtcNow;

        // 🔹 Một giáo viên có thể chủ nhiệm nhiều lớp
        public ICollection<LopHoc> LopChuNhiems { get; set; } = new List<LopHoc>();
    }
}
