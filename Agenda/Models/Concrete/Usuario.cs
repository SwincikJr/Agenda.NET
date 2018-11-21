using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Models.Concrete
{
    
    public class Usuario
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Favor, informar endereço de e-mail")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Favor, informar nome de usuário")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Favor, informar senha")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Confirmado { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}