using System;
using System.Collections.Generic;

namespace draft;

public partial class Teacher
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }

    public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    public virtual Department Department { get; set; } = null!;
}
