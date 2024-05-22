using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace First_project.Models
{
    public class Register
    {
        public string? UserName { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? Specialization { get; set; }

        public decimal? RoleId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;

    }
}
