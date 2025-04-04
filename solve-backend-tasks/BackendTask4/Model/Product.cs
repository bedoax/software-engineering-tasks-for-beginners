namespace BackendTask4.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public List<ProductCategory> ProductCategories { get; set; } = new();
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
