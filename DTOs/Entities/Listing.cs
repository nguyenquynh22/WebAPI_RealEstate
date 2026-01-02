using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.Entities
{
    public class Listing
    {
        public Guid ListingId { get; set; }
        public Guid? PropertyId { get; set; }
        public Guid? AgentId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ProjectAreaPropertyTypeId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal? AreaSqM { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ImagesJson { get; set; } 
        public string VideosJson { get; set; } 
        public string Status { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsForRent { get; set; }
        public string RentDurationUnit { get; set; }
        public DateTime PostedAt { get; set; }
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsSoldOrRented { get; set; }
        public decimal ComputedPrice { get; set; }
    }
}
