using System;
using System.Collections.Generic;

namespace UnivDBWebApplication;

public partial class CourseAssignment
{
    public int Id { get; set; }
    public int CourseId { get; set; }

    public int TeacherId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
