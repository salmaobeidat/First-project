using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace First_project.Models;

public partial class Testimonial
{
    public decimal TestimonialId { get; set; }

    public DateTime? CreationDate { get; set; }
    [Display(Name = "Testimonial Content")]
    public string? TestimonialContent { get; set; }

    public decimal? UserId { get; set; }

    public decimal? StatusId { get; set; }

    public virtual Status? Status { get; set; }

    public virtual User? User { get; set; }
}
