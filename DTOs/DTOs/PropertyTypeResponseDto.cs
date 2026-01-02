using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class PropertyTypeResponseDto
    {
        public int PropertyTypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
    }
}