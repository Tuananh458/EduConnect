using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    // Đây là câu hỏi “nằm trong” học liệu
    public class CauHoiHocLieu
    {
        [Key]
        public int Id { get; set; }

        // FK tới HocLieu
        public int HocLieuId { get; set; }

        [ForeignKey(nameof(HocLieuId))]
        public HocLieu? HocLieu { get; set; }

        // Tiêu đề / nội dung câu hỏi
        [Required]
        public string NoiDung { get; set; } = string.Empty;

        // Loại câu hỏi: TN_1DAPANDUNG, TN_NHIEUDAPANDUNG, DUNGSAI, ...
        [MaxLength(50)]
        public string LoaiCauHoi { get; set; } = "TN_1DAPANDUNG";

        // Mức độ: Nhận biết / Thông hiểu / Vận dụng / Vận dụng cao
        [MaxLength(50)]
        public string DoKho { get; set; } = "Nhận biết";

        // Điểm cho câu hỏi này
        public decimal Diem { get; set; } = 1m;

        // 4 đáp án cơ bản giống bạn đang làm giao diện
        public string? DapAnA { get; set; }
        public string? DapAnB { get; set; }
        public string? DapAnC { get; set; }
        public string? DapAnD { get; set; }

        // Lưu đáp án đúng (A/B/C/D) — với câu nhiều đáp án đúng bạn có thể lưu JSON sau
        [MaxLength(10)]
        public string? DapAnDung { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
    }
}
