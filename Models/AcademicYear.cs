using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class AcademicYear
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime ClosureDate { get; set; }

    public DateTime FinalClosureDate { get; set; }

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
}
