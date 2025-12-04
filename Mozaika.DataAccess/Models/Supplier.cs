using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
