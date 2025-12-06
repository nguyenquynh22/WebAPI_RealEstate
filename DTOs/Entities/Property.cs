using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.Entities
{
    public class Property
    {
        public Guid PropertyId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AreaId { get; set; }
        public string UnitNumber { get; set; }
        public string BlockOrTower { get; set; }
        public int? Floor { get; set; }
        public string Status { get; set; }
        public Guid? OwnerId { get; set; }
        public decimal? OriginalAreaSqM { get; set; }
        public decimal? OriginalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
