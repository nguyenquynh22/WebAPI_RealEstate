using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class UserFilterDto
    {
        public string Keyword { get; set; } // Tìm kiếm theo UserName, Email, Phone
        public string Role { get; set; } // Lọc theo Role ('admin', 'agent', 'customer')
        public string KycStatus { get; set; } // Lọc theo trạng thái KYC

        // Phân trang
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
