using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace First_project.Models;

public partial class User
{
    public decimal UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Specialization { get; set; }

    public decimal? RoleId { get; set; }
    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; }

    public string? Profileimagepath { get; set; }

    public virtual Credential? Credential { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual Visainfo? Visainfo { get; set; }
}
