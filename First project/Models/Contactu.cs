using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace First_project.Models;

public partial class Contactu
{
    public decimal ContactId { get; set; }

    public string? Subject { get; set; }
    [Required]
    public string Email { get; set; } = null!;

    public string? MessageContent { get; set; }
}
