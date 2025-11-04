using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Models.HocLieu
{
    public class HocLieu
    {
        [Key]
        public int Id { get; set; }

        // Tên hiển thị trên UI
        [Required]
        [MaxLength(255)]
        public string TenHocLieu { get; set; } = string.Empty;

        // Mã loại để lọc: TN, DT, LT, ...
        [Required]
        [MaxLength(20)]
        public string MaLoaiHocLieu { get; set; } = string.Empty;

        // Tên loại để hiển thị
        [MaxLength(255)]
        public string? TenLoaiHocLieu { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        // "Tự tạo", "Sao chép"
        [MaxLength(50)]
        public string NguonTao { get; set; } = "Tự tạo";

        public bool LaHocLieuTuDo { get; set; } = false;
        public bool LaHocLieuAn { get; set; } = false;

        // nếu bạn có bảng Khóa học thì chỗ này là FK
        public int? KhoaHocId { get; set; }
        public string? TenKhoaHoc { get; set; }

        // quan hệ 1-n câu hỏi
        public ICollection<CauHoiHocLieu> CauHois { get; set; } = new List<CauHoiHocLieu>();
    }
}
