using System.ComponentModel.DataAnnotations;

namespace Kuva.Accounts.Service.Models
{
    public class RegisterClientModel
    {
        [Required(ErrorMessage = "Nome é necessário"), MinLength(12)]
        public string Name { get; set; }
        [Required, EmailAddress(ErrorMessage = "E-mail é inválido")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É necessário informar uma senha."), 
         MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres."),]
        public string Password { get; set; }
    }
}