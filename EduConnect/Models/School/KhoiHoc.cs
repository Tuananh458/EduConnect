using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.School
{
    [Table("KHOIHOC")]
    public class KhoiHoc
    {
        [Key]
        [Column("maKhoiHoc")]
        public int MaKhoiHoc { get; set; }

        [Required]
        [Column("tenKhoiHoc")]
        [MaxLength(50)]
        public string TenKhoiHoc { get; set; } = string.Empty;
        public ICollection<LopHoc>? LopHocs { get; set; }

    }
}
