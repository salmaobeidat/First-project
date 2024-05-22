using System;
using System.Collections.Generic;

namespace First_project.Models;

public partial class Order
{
    public decimal OrderId { get; set; }

    public decimal? UserId { get; set; }

    public decimal? PaymentId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? RecipeId { get; set; }

    public virtual Visainfo? Payment { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual User? User { get; set; }
}
