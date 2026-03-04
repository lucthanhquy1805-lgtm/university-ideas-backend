using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class Document
{
    public int Id { get; set; }

    public int IdeaId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }

    public virtual Idea Idea { get; set; } = null!;
}
