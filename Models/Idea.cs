using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class Idea
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public int? TopicId { get; set; } // Khóa ngoại trỏ tới Topic (Cho phép null nếu có ý tưởng không thuộc Topic nào)
    public virtual Topic? Topic { get; set; } // Navigation property

    public int AcademicYearId { get; set; }

    public bool? IsAnonymous { get; set; }

    public int? ViewCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AcademicYear AcademicYear { get; set; } = null!;
    public int? TopicId { get; set; }
    public virtual Topic? Topic { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();


    public virtual User User { get; set; } = null!;
}
