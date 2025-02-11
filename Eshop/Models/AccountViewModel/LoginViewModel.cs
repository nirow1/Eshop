using System.ComponentModel.DataAnnotations;

namespace Eshop.Models.AccountViewModel
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Pro přihlášení musíte zadat email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email {  get; set; }


        [Required(ErrorMessage = "Pro přihlášení musíte zadat heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [Display(Name = "Pamatuj si mě")]
        public string RememberMe {  get; set; }
    }
}
