using System;
using System.Collections.Generic;

namespace EduConnect.Shared.DTOs.HocLieu
{
    // Đề thi học liệu
    public class DeThiHocLieuDto
    {
        public int HocLieuId { get; set; }
        public string TenHocLieu { get; set; } = string.Empty;
        public List<CauHoiHocLieuDto> CauHoi { get; set; } = new();
    }

    // Tạo bài làm
    public class TaoBaiLamRequest
    {
        public int HocLieuId { get; set; }
        public int HocSinhId { get; set; }
    }

    // Nộp bài
    public class NopBaiRequest
    {
        public int BaiLamId { get; set; }
        public int HocLieuId { get; set; }
        public int HocSinhId { get; set; }
        public List<CauTraLoiDto> CauTraLoi { get; set; } = new();
    }

    public class CauTraLoiDto
    {
        public int CauHoiId { get; set; }
        public string DapAnChon { get; set; } = string.Empty;
    }

    // Kết quả bài làm
    public class KetQuaBaiLamDto
    {
        public int BaiLamId { get; set; }
        public int HocLieuId { get; set; }
        public double? Diem { get; set; }
        public DateTime? ThoiGianNop { get; set; }
        public List<CauTraLoiKetQuaDto> CauTraLoi { get; set; } = new();
    }

    public class CauTraLoiKetQuaDto
    {
        public int CauHoiId { get; set; }
        public string NoiDung { get; set; } = string.Empty;
        public string DapAnDung { get; set; } = string.Empty;
        public string DapAnChon { get; set; } = string.Empty;
        public bool Dung { get; set; }
    }

    // Bảng điểm học liệu
    public class BaiLamHocLieuDto
    {
        public int BaiLamId { get; set; }
        public string TenHocSinh { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double? Diem { get; set; }
        public DateTime? ThoiGianNop { get; set; }
    }
}
