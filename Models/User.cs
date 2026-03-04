using System;
using System.Collections.Generic;

namespace UniversityIdeas.API.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int DepartmentId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
