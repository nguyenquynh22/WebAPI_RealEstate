namespace Common_DTOs.DTOs
{
    public class NewsFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string Status { get; set; } = string.Empty;  // "" => all
        public string Keyword { get; set; } = string.Empty; // "" => all
        public int IsHighlight { get; set; } = -1;          // -1 all, 0 false, 1 true
        public int IsExternal { get; set; } = -1;           // -1 all, 0 false, 1 true
    }
}
