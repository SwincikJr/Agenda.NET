using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Mail;
using Agenda.Models.Concrete;

namespace Agenda.Models.Concrete
{
    public static class EmailProcessor
    {
        public static void Process(Usuario usuario, string rootUrl)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("youremail@gmail.com", "password");
                smtpClient.EnableSsl = true;

                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                //smtpClient.PickupDirectoryLocation = @"c:\Users\user\Documents\Projetos\Agenda\agenda_emails";
                //smtpClient.EnableSsl = false;

                StringBuilder body = new StringBuilder();
                string assunto;

                if (rootUrl != null)
                {
                    body.AppendLine("Olá, " + usuario.Username + "!")
                        .AppendLine("Para confirmar seu endereço de e-mail e começar a usar o aplicativo, acesse a rota abaixo: ")
                        .AppendLine("")
                        .AppendLine(rootUrl + "/Account/ConfirmarEmail/" + usuario.UserID.ToString());
                    assunto = "Confirmação de endereço de e-mail";

                }
                else
                {
                    body.AppendLine("Olá, " + usuario.Username + "!")
                        .AppendLine("Alteramos sua senha para que você possa acessar sua agenda.")
                        .AppendLine("Não se esqueça de altera-la para uma senha melhor!")
                        .AppendLine()
                        .AppendLine("Nova senha: " + usuario.Password.ToString());
                    assunto = "Recuperação de senha";
                }

                MailMessage mailMessage = new MailMessage("youremail@gmail.com", usuario.Email, assunto, body.ToString());

                mailMessage.BodyEncoding = Encoding.UTF8;
                smtpClient.Send(mailMessage);

            }
        }
    }
}