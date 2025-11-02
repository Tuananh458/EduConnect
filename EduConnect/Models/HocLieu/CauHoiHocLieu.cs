using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Models.HocLieu
{
    public class CauHoiHocLieu
    {
        [Key] // 🔑 Định nghĩa khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự tăng
        public int MaCauHoi { get; set; }

        [Required]
        public string NoiDung { get; set; } = string.Empty;

        [Required]
        public LoaiCauHoiHocLieu Loai { get; set; }

        [MaxLength(50)]
        public string DoKho { get; set; } = "Nhận biết";

        public string? GiaiThich { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [JsonIgnore]
        public ICollection<HocLieuCauHoi> HocLieuCauHois { get; set; } = new HashSet<HocLieuCauHoi>();
    }
}
