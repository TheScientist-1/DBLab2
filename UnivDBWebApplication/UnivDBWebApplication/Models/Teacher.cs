using System;
using System.Collections.Generic;

namespace UnivDBWebApplication;

public partial class Teacher
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;
}
