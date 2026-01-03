using System;

namespace Common_DTOs.DTOs
{
    public class CommentsFilterDto
    {
        public Guid NewsId { get; set; } = Guid.Empty;
        public Guid ListingId { get; set; } = Guid.Empty;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}


// truyền newid lấy coment theo new 
// truyền listingid lấy comment theo listing -> bán 
//nhớ 