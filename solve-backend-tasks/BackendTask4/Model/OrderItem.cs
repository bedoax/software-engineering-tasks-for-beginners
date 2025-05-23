﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendTask4.Model
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; } = new();
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}