using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_project.Models;

public partial class Category
{
    public decimal CategoryId { get; set; }
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; }

    [NotMapped]
    [Display(Name = "Category Image")]
    public IFormFile CategoryImage { get; set; }
    public string? CategoryImagePath { get; set; }


    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
