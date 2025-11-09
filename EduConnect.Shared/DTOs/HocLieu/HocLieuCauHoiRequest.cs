using System.Collections.Generic;

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class HocLieuCauHoiRequest
    {
        public int HocLieuId { get; set; }
        public List<int> CauHoiIds { get; set; } = new();
    }
}
