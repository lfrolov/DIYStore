using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DIYStoreWeb.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин Пользователя")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
