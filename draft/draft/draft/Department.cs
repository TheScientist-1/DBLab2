using System;
using System.Collections.Generic;

namespace draft;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
