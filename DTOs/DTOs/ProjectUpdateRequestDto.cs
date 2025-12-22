using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class ProjectUpdateRequestDto
    {
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Developer { get; set; }
    }
}
