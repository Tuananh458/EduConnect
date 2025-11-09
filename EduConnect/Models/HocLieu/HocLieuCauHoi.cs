using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    public class HocLieuCauHoi
    {
        [Key]
        public int Id { get; set; }

        // === Khóa ngoại đến Học Liệu ===
        [Required]
        [ForeignKey(nameof(HocLieu))]
        public int HocLieuId { get; set; }

        // === Khóa ngoại đến Câu hỏi ===
        [Required]
        [ForeignKey(nameof(CauHoi))]
        public int CauHoiId { get; set; }

        // Thứ tự hiển thị câu hỏi trong học liệu
        [Range(1, int.MaxValue)]
        public int ThuTu { get; set; } = 1;

        // Ngày thêm câu hỏi vào học liệu
        public DateTime NgayThem { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public HocLieu HocLieu { get; set; } = null!;
        public CauHoiHocLieu CauHoi { get; set; } = null!;
    }
}
