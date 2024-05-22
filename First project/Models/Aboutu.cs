using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_project.Models;

public partial class Aboutu
{
    public decimal AboutUs { get; set; }

    public string? AboutusImagePath { get; set; }

    [NotMapped]
    [Display(Name = "About us Image")]
    public IFormFile ImageFile { get; set; }

    [Display(Name = "About us Header")]
    public string? Header { get; set; }

    [Display(Name = "About us content")]
    public string? AboutusContent { get; set; }
}
