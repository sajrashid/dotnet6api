using System;

namespace API.DTOs
{
    public class ProductsDto
    {
        public int Id { get; set; }
    public string Company { get; set; }
    public string Phone { get; set; }
    public decimal Price { get; set; }
    public bool InStock { get; set; }
    public int StockCount { get; set; }
    public DateTime NewStockDate { get; set; }

    }
}
