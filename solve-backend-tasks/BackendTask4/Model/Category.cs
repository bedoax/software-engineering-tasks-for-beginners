﻿namespace BackendTask4.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

       
        public List<ProductCategory> ProductCategories { get; set; } = [];
    }
}
