namespace ECommerceBackend.DTO___Mapping
{
    public class DashBoardDto
    {
        public int ValidUsers { get; set; } = 0;
        public int InValidUsers { get; set; } = 0;
        public int ValidProducts { get; set; } = 0;
        public int InValidProducts { get; set; } = 0;
        public int TotalOrders { get; set; } = 0;
        public decimal? TotalRevenue { get; set; } = 0;
    }
}
