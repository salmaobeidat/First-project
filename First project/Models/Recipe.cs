using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_project.Models;

public partial class Recipe
{
    public decimal RecipeId { get; set; }
    [DisplayName("Recipe Name")]
    public string? RecipeName { get; set; }
    [DisplayName("Time takes")]
    public decimal? TimeTakes { get; set; }

    public decimal? Price { get; set; }
    [DisplayName("Total calories")]
    public decimal? TotalCalories { get; set; }
    [NotMapped]
    public IFormFile RecipeImage { get; set; }

    public string? Description { get; set; }
    [DisplayName("Recipe PDF")]
    public string? RepcipePdfPath { get; set; }

    public decimal? UserId { get; set; }
    [DisplayName("Category name")]
    public decimal? CategoryId { get; set; }

    public decimal? StatusId { get; set; }
    [DisplayName("Recipe Image")]
    public string? RecipeImagePath { get; set; }

    public string Ingredients { get; set; } = null!;

    public DateTimeOffset? Creationdate { get; set; }
    public virtual Category? Category { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Status? Status { get; set; }

    public virtual User? User { get; set; }
}
