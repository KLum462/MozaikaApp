using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class MaterialHistory
{
    public int HistoryId { get; set; }

    public int MaterialId { get; set; }

    public DateTime? OperationDate { get; set; }

    public string? OperationType { get; set; }

    public decimal? QuantityChange { get; set; }

    public string? Comment { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Material Material { get; set; } = null!;
}
