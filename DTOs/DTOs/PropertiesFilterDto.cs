using System;

namespace Common_DTOs.DTOs
{
    public class PropertiesFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public Guid ProjectId { get; set; }
        public Guid AreaId { get; set; }
        public Guid OwnerId { get; set; }

        public string Status { get; set; } = string.Empty;  
        public string UnitNumber { get; set; } = string.Empty;
    }
}
