using System;

namespace Common_DTOs.DTOs
{
    public class ListingsFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public Guid ProjectId { get; set; }
        public Guid AreaId { get; set; }
        public Guid AgentId { get; set; }
        public Guid PropertyId { get; set; }

        public string Status { get; set; } = string.Empty;
        public string Keyword { get; set; } = string.Empty;

        public int IsForRent { get; set; } = -1;   // -1=ALL, 0=false, 1=true
        public int IsFeatured { get; set; } = -1;  // -1=ALL, 0=false, 1=true
    }
}
