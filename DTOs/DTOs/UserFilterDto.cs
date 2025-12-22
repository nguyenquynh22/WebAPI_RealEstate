using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class UserFilterDto
    {
        public string Keyword { get; set; } 
        public string Role { get; set; } 
        public string KycStatus { get; set; } 

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
