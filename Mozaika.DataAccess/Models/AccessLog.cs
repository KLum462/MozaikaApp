using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class AccessLog
{
    public int LogId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? EventTime { get; set; }

    public string? EventType { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
