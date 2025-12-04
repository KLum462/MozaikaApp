using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class Partner
{
    public int PartnerId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? Rating { get; set; }

    public int? DiscountPercent { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
