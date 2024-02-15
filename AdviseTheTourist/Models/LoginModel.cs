using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AdviseTheTourist.Models
{
    public enum LoginState
    {
        INITIAL,
        NOTFOUND,
        WRONGPASSWORD,
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [NotNull]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [NotNull]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
