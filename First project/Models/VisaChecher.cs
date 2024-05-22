using System;
using System.Collections.Generic;

namespace First_project.Models;

public partial class VisaChecher
{
    public decimal VisaChecherId { get; set; }

    public decimal Cvc { get; set; }

    public string CardHolderName { get; set; } = null!;

    public decimal CardNumber { get; set; }

    public decimal? Balance { get; set; }
}
