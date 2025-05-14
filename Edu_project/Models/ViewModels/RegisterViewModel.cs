// Models/ViewModels/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace EdukateMvc.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
