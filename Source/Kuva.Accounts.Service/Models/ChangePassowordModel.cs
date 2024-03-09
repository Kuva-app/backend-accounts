using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kuva.Accounts.Service.Models
{
    public class ChangePassowordModel
    {
        [Required(ErrorMessage = "Necessário informar o código de confirmação"), MinLength(8)]
        public string ConfirmationCode { get; set; }
        
        [Required(ErrorMessage = "Necessário informar a nova senha"), PasswordPropertyText(true)]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "Necessário informar a confirmação da senhah"), 
         Compare(nameof(NewPassword), ErrorMessage = "A confirmação da senha deve ser igual a nova senha"),
        PasswordPropertyText(true)]
        public string ConfirmPassword { get; set; }
    }
}