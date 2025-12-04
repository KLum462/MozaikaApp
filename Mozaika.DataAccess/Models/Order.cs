using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int PartnerId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? PaymentDeadline { get; set; }

    public virtual Employee? Manager { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Partner Partner { get; set; } = null!;
}
