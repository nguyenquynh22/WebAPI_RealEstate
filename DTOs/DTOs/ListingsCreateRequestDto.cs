using System;

namespace Common_DTOs.DTOs
{
    public class ListingsCreateRequestDto
    {
        public Guid PropertyId { get; set; }
        public Guid AgentId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AreaId { get; set; }
        public Guid ProjectAreaPropertyTypeId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string PriceUnit { get; set; } = "VND";

        public decimal DiscountAmount { get; set; }
        public decimal AreaSqM { get; set; }

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string ImagesJson { get; set; } = "[]";
        public string VideosJson { get; set; } = "[]";

        public string Status { get; set; } = "Pending";

        public bool IsAvailable { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public bool IsForRent { get; set; } = false;

        public string RentDurationUnit { get; set; } = string.Empty;
    }
}
