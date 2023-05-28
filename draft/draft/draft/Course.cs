using System;
using System.Collections.Generic;

namespace draft;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public int CreditsNumber { get; set; }

    public int DepartmentId { get; set; }

    public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Studying> Studyings { get; set; } = new List<Studying>();
}
