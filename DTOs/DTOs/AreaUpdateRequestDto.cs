using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class AreaUpdateRequestDto
    {
        public Guid AreaId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
