using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mozaika.DataAccess.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductMaterials = new HashSet<ProductMaterial>();
            OrderDetails = new HashSet<OrderDetail>(); // <-- ЭТО БЫЛО ПРОПУЩЕНО
        }

        // ВАЖНО: Пишем ProductId (как требует Context), а не ProductID
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        // ВАЖНО: Пишем CategoryId (как требует Context)
        public int? CategoryId { get; set; }

        public decimal? MinCost { get; set; }
        public int? StandardProductionTime { get; set; }

        // --- Новые поля ---
        public string? ProductArticleNumber { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductManufacturer { get; set; }
        public int? ProductQuantityInStock { get; set; }
        public string? ProductUnit { get; set; }

        // --- Навигационные свойства ---
        public virtual ProductCategory? Category { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }

        // Исправляет ошибку "Product не содержит OrderDetails"
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}