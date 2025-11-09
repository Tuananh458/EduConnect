using System;
using System.Collections.Generic;

namespace EduConnect.Shared.DTOs.HocSinh
{
    public class HocSinhPreviewDto
    {
        public string HoVaTen { get; set; } = "";
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "";
        public string MaDinhDanh { get; set; } = "";
        public DateTime? NgaySinh { get; set; }
        public string? Email { get; set; }
        public bool HopLe { get; set; }
        public string? Loi { get; set; }
    }

    public class ImportPreviewResponse
    {
        public int MaLopHoc { get; set; }
        public string? Prefix { get; set; }
        public List<HocSinhPreviewDto> Items { get; set; } = new();
    }

    public class ImportConfirmRequest
    {
        public int MaLopHoc { get; set; }
        public List<HocSinhPreviewDto> Items { get; set; } = new();
        public string Action { get; set; } = "createOnly"; // createOnly, addToClass, transferToClass
    }

    public class ImportConfirmResult
    {
        public int Created { get; set; }
        public int AddedToClass { get; set; }
        public int Transferred { get; set; }
        public int Skipped { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
