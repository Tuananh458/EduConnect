using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    public class ChiTietBaiLamHocLieu
    {
        [Key]
        public int Id { get; set; }

        public int BaiLamHocLieuId { get; set; }
        [ForeignKey(nameof(BaiLamHocLieuId))]
        public BaiLamHocLieu BaiLam { get; set; }

        public int CauHoiId { get; set; }

        [MaxLength(5)]
        public string? DapAnChon { get; set; }

        [MaxLength(5)]
        public string? DapAnDung { get; set; }

        public double Diem { get; set; }
    }
}
