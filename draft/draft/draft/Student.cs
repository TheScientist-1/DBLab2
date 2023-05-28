using System;
using System.Collections.Generic;

namespace draft;

public partial class Student
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime EnrollmentDate { get; set; }

    public virtual ICollection<Studying> Studyings { get; set; } = new List<Studying>();
}
