using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
}
