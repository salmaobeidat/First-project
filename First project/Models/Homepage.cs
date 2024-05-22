using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_project.Models;

public partial class Homepage
{
    public decimal HomeId { get; set; }
    [DisplayName("Background image")]
    public string? BackgroundImagePath { get; set; }
    [NotMapped]
    public IFormFile BackgroundImage { get; set; }

    [DisplayName(" Header")]

    public string? SliderHeader { get; set; }
    [DisplayName(" Paragraph")]

    public string? SliderParagraph { get; set; }
}
