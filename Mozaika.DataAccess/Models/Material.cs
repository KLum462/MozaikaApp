using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public string? Unit { get; set; }

    public decimal? QuantityInStock { get; set; }

    public decimal? CostPerUnit { get; set; }

    public int? SupplierId { get; set; }

    public virtual ICollection<MaterialHistory> MaterialHistories { get; set; } = new List<MaterialHistory>();

    public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();

    public virtual Supplier? Supplier { get; set; }
}
