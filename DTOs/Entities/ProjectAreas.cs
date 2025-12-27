using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.Entities
{
    public class ProjectAreas
    {
        public Guid AreaId { get; set; }
        public Guid ProjectId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
