using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.Entities
{
    public class ProjectAreaPropertyTypes
    {
        public Guid ProjectAreaPropertyTypeId { get; set; }
        public Guid AreaId { get; set; }
        public int PropertyTypeId { get; set; }
        public string? AreaName { get; set; }
        public string? TypeName { get; set; }
    }
}
