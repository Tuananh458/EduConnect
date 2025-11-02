using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduConnect.Models.HocLieu
{
    public class HocLieu
    {
        [Key] // 🔑 Khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaHocLieu { get; set; }

        [Required]
        [MaxLength(255)]
        public string TenHocLieu { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string TheLoai { get; set; } = string.Empty;

        public bool DaDuyet { get; set; } = false;
        public bool HienThi { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string? NguoiTao { get; set; }

        [JsonIgnore]
        public ICollection<HocLieuCauHoi> HocLieuCauHois { get; set; } = new HashSet<HocLieuCauHoi>();
    }
}
