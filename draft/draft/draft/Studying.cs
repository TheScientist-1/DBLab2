using System;
using System.Collections.Generic;

namespace draft;

public partial class Studying
{
    public int StudyingId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int Grade { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
