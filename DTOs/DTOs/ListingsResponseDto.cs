using System;

namespace Common_DTOs.DTOs
{
    public class ListingsResponseDto
    {
        public Guid ListingId { get; set; }

        public Guid PropertyId { get; set; }
        public Guid AgentId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AreaId { get; set; }
        public Guid ProjectAreaPropertyTypeId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string PriceUnit { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }

        public decimal AreaSqM { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string ImagesJson { get; set; } = "[]";
        public string VideosJson { get; set; } = "[]";

        public string Status { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsForRent { get; set; }

        public string RentDurationUnit { get; set; } = string.Empty;

        public DateTime PostedAt { get; set; }
        public int Views { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // computed columns (nếu DB có). Nếu DB bạn KHÔNG có 2 cột này thì bạn xóa 2 property này đi.
        public bool IsSoldOrRented { get; set; }
        public decimal ComputedPrice { get; set; }
    }
}
