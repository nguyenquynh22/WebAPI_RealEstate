using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class ProjectAreaPropertyTypeCreateRequestDto
    {
        public Guid AreaId { get; set; }
        public int PropertyTypeId { get; set; }
    }
}
