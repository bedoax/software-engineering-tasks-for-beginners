namespace BackendTask4.Model
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}