using System;
using System.Collections.Generic;

namespace First_project.Models;

public partial class Status
{
    public decimal StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
