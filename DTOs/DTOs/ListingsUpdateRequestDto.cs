using System;

namespace Common_DTOs.DTOs
{
    public class ListingsUpdateRequestDto : ListingsCreateRequestDto
    {
        public Guid ListingId { get; set; }
    }
}
