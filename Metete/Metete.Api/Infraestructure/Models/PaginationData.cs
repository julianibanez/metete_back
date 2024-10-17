namespace Metete.Api.Infraestructure.Models
{
    public class PaginationData
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string? SortField { get; set; }

        public string? SortOrder { get; set; }
    }
}
