using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Concrete
{
    public class AlterarSenhaModelView
    {
        [Required(ErrorMessage = "Favor, informar a senha atual.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Favor, informar uma nova senha.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Favor, confirmar a nova senha desejada.")]
        [Compare("NewPassword", ErrorMessage = "Confirmação de nova senha não confere.")]
        public string ConfirmPassword { get; set; }
    }
}