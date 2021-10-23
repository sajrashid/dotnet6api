// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Company { get; set; } = null!;

        public string? Phone { get; set; } = null!;

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public int StockCount { get; set; }

        public DateTime NewStockDate { get; set; }
    }
}
