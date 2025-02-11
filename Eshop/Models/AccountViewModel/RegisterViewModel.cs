using System.ComponentModel.DataAnnotations;

namespace Eshop.Models.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email je povinný")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinné")]
        [StringLength(100, ErrorMessage = "Heslo musí být alespoň 8 znaků dlouhé"), MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password {  get; set; }


        [Required(ErrorMessage = "Heslo je povinné")]
        [Compare("Password", ErrorMessage = "Zadaná hesla se neshodují"), MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "potvrzení hesla")]
        public string ConfirmPassword { get; set; }
    }
}
