using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    [Table("HOCLIEU")]
    public class HocLieu
    {
        [Key]
        [Column("hocLieuId")]
        public int HocLieuId { get; set; }
        [Required]
        [MaxLength(255)]
        [Column("tenHocLieu")]
        public string TenHocLieu { get; set; } = string.Empty;


        [Required]
        [MaxLength(20)]
        [Column("maLoaiHocLieu")]
        public string MaLoaiHocLieu { get; set; } = string.Empty;


        [MaxLength(255)]
        [Column("tenLoaiHocLieu")]
        public string? TenLoaiHocLieu { get; set; }


        [Column("ngayTao")]
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;


        [MaxLength(50)]
        [Column("nguonTao")]
        public string NguonTao { get; set; } = "Tự tạo";

        [Column("laHocLieuTuDo")]
        public bool LaHocLieuTuDo { get; set; } = false;

        [Column("laHocLieuAn")]
        public bool LaHocLieuAn { get; set; } = false;

  
        [Column("khoaHocId")]
        public int? KhoaHocId { get; set; }

        [Column("tenKhoaHoc")]
        [MaxLength(255)]
        public string? TenKhoaHoc { get; set; }
        [Column("nguoiTaoId")]
        public Guid NguoiTaoId { get; set; }

        // 🔗 Khóa ngoại đến bảng User (người tạo học liệu)
        [ForeignKey(nameof(NguoiTaoId))]
        public NguoiDung? NguoiTao { get; set; }


        public ICollection<CauHoiHocLieu> CauHois { get; set; } = new List<CauHoiHocLieu>();

        // 📋 1 Học liệu có nhiều bài làm
        public ICollection<BaiLamHocLieu> BaiLams { get; set; } = new List<BaiLamHocLieu>();

        // 🔗 Liên kết học liệu – câu hỏi (nhiều–nhiều)
        public ICollection<HocLieuCauHoi> HocLieuCauHois { get; set; } = new List<HocLieuCauHoi>();
    }
}
