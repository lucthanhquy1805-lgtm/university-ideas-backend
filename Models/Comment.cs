using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int IdeaId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public bool? IsAnonymous { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Idea Idea { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
