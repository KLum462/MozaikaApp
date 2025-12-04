using System;
using System.Collections.Generic;

namespace Mozaika.DataAccess.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }

    public string? CardNumber { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

    public virtual ICollection<MaterialHistory> MaterialHistories { get; set; } = new List<MaterialHistory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
