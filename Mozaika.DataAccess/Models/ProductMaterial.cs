using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class ProductMaterial
{
    public int ProductId { get; set; }

    public int MaterialId { get; set; }

    public decimal? QuantityRequired { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
