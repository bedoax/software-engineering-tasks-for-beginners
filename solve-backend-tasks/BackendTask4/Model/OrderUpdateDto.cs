namespace BackendTask4.Model
{
    public class OrderUpdateDto
    {
        public int UserId { get; set; }
        public List<OrderItemUpdateDto> OrderDetails { get; set; } = new();
    }

    public class OrderItemUpdateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}