using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Models.HocLieu
{
    public class BaiLamHocLieu
    {
        [Key]
        public int Id { get; set; }

        public int HocLieuId { get; set; }

        // FK nếu bạn có bảng học sinh
        public int? HocSinhId { get; set; }

        [Required]
        [MaxLength(255)]
        public string TenHocSinh { get; set; } = string.Empty;

        public DateTime ThoiGianBatDau { get; set; } = DateTime.UtcNow;
        public DateTime? ThoiGianNop { get; set; }

        public double TongDiem { get; set; }

        public ICollection<ChiTietBaiLamHocLieu> ChiTietBaiLams { get; set; } = new List<ChiTietBaiLamHocLieu>();
    }
}
