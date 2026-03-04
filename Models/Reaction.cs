using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class Reaction
{
    public int IdeaId { get; set; }

    public int UserId { get; set; }

    public int ReactionType { get; set; }

    public virtual Idea Idea { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
