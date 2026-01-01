using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.DTOs
{
    public class PropertiesUpdateRequestDto
    {
        public Guid PropertyId { get; set; }

        public Guid ProjectId { get; set; }      
        public Guid AreaId { get; set; }         
        public string UnitNumber { get; set; } = string.Empty;     
        public string BlockOrTower { get; set; } = string.Empty;   
        public int Floor { get; set; }           
        public string Status { get; set; } = "Available";
        public Guid OwnerId { get; set; }        
        public decimal OriginalAreaSqM { get; set; }
        public decimal OriginalPrice { get; set; }
    }
}
