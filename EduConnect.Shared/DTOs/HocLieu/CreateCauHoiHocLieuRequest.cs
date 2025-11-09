using System.ComponentModel.DataAnnotations;

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class CreateCauHoiHocLieuRequest
    {
        [Required]
        public int HocLieuId { get; set; }

        [Required, MaxLength(1000)]
        public string TieuDe { get; set; }

        public string NoiDung { get; set; } = string.Empty;

        [Required]
        public string LoaiCauHoi { get; set; } = "TracNghiem"; // hoặc Tự luận, Đúng/Sai

        [Required]
        public string DoKho { get; set; } = "Nhận biết";

        [Range(0.5, 10)]
        public double Diem { get; set; }

        // 🔹 Các đáp án (trắc nghiệm A/B/C/D)
        [MaxLength(255)]
        public string? DapAnA { get; set; }

        [MaxLength(255)]
        public string? DapAnB { get; set; }

        [MaxLength(255)]
        public string? DapAnC { get; set; }

        [MaxLength(255)]
        public string? DapAnD { get; set; }

        // 🔹 Đáp án đúng
        [Required]
        public string DapAnDung { get; set; } = string.Empty;
    }
}
