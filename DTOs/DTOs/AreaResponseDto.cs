using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class AreaResponseDto
    {
        public Guid AreaId { get; set; }
        public Guid ProjectId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CreatedAt { get; set; } = string.Empty; 
        public string? UpdatedAt { get; set; }
    }
}
