using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agenda.Models.Abstract;
using Agenda.Models.Concrete;
using System.Data.Entity;
using System.Text;
using System.Web.Routing;

namespace Agenda.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IEventoRepositorio repositorio;
        private IUsuarioRepositorio usuarioRepositorio;

        public HomeController(IEventoRepositorio repo, IUsuarioRepositorio userRepo)
        {
            this.repositorio = repo;
            this.usuarioRepositorio = userRepo;
        }

        public ViewResult Index()
        {
            List<Evento> eventos = new List<Evento>();

            foreach (Evento evento in repositorio.Eventos)
            {
                eventos.Add(new Evento { EventoID = evento.EventoID, UserID = evento.UserID, dataHora = (DateTime)evento.dataHora, Descricao = evento.Descricao });
            }

            IEnumerable<Evento> eventosHoje = eventos
                .Where(x => x.UserID == (int)Session["ID"])
                .Where(x => x.dataHora.Date == DateTime.Now.Date)
                .OrderBy(e => (e.dataHora.Hour * 60 * 60 + e.dataHora.Minute * 60 + e.dataHora.Second));

            Session["CurrentPage"] = "Index";
            return View(eventosHoje);
        }

        public ActionResult Config()
        {
            return View();
        }

        public ActionResult Sair()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ViewResult AlterarSenha()
        {
            return View(new AlterarSenhaModelView());
        }
        
        [HttpPost]
        public ActionResult AlterarSenha(AlterarSenhaModelView model)
        {
            if(ModelState.IsValid)
            {
                int id = (int)Session["ID"];
                Usuario usuario = usuarioRepositorio.Usuarios
                    .FirstOrDefault(u => u.UserID == id);
                if (usuario == null) return View("SecurityFail");
                if (usuario.Password == model.Password
                        && usuario.Password != model.NewPassword)
                {
                    usuario.Password = model.NewPassword;
                    usuarioRepositorio.EditarUsuario(usuario);
                    TempData["InfoAlterada"] = "Senha alterada com sucesso!";
                    return Redirect("/Home/Config");
                }
                else {
                    if (usuario.Password != model.Password)
                        ModelState.AddModelError("", "Senha atual informada incorretamente.");
                    if (usuario.Password == model.NewPassword)
                        ModelState.AddModelError("", "A nova senha deve ser diferente da senha atual.");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult AlterarNome()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlterarNome(string novoNome)
        {
            if (!string.IsNullOrEmpty(novoNome))
            {
                int id = (int)Session["ID"];
                Usuario usuario = usuarioRepositorio.Usuarios
                                    .FirstOrDefault(u => u.UserID == id);
                if (usuario != null)
                {
                    if (usuario.Username != novoNome)
                    {
                        usuario.Username = novoNome;
                        Session["Usuario"] = novoNome;
                        usuarioRepositorio.EditarUsuario(usuario);
                        TempData["InfoAlterada"] = "Nome de usuário alterado com sucesso!";
                        return Redirect("/Home/Config");
                    }
                    else
                    {
                        ModelState.AddModelError("", "O novo nome de usuário deve ser diferente do nome atual.");
                    }
                }
                else
                {
                    return View("SecurityFail");
                }
            }
            else
            {
                ModelState.AddModelError("", "Favor, informar um novo nome de usuário.");
            }
            return View();
        }

        [HttpGet]
        public ActionResult AlterarFoto()
        {
            int id = (int)Session["ID"];
            Usuario usuario = usuarioRepositorio.Usuarios
                .FirstOrDefault(u => u.UserID == id);
            if (User != null) return View(usuario);
            else return View("SecurityFail");
        }

        [HttpPost]
        public ActionResult AlterarFoto(HttpPostedFileBase image)
        {
            int id = (int)Session["ID"];
            Usuario usuario = usuarioRepositorio.Usuarios
                .FirstOrDefault(u => u.UserID == id);
                if (image != null)
                {
                    usuario.ImageMimeType = image.ContentType;
                    usuario.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(usuario.ImageData, 0, image.ContentLength);
                    usuarioRepositorio.EditarUsuario(usuario);
                    Session["Image"] = File(usuario.ImageData, usuario.ImageMimeType);
                    TempData["InfoAlterada"] = "Foto alterada com sucesso!";
                    return Redirect("/Home/Config");
                }
                else
                {
                    return View("SecurityFail");
                }
        }
    }
}

/*

SqlConnection connection = new SqlConnection
            (
                System.Web.Configuration.WebConfigurationManager
                    .ConnectionStrings["EFDbContext"].ConnectionString
            );
            string query = @"SELECT * FROM Eventos WHERE 
                                DAY(DataHora) = DAY(@Today) AND
                                MONTH(DataHora) = MONTH(@Today) AND
                                YEAR(DataHora) = YEAR(@Today)
                                ORDER BY DataHora";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Today", DateTime.Now);
                command.ExecuteNonQuery();
                List<Evento> eventos = new List<Evento>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eventos.Add
                    (
                        new Evento
                        {
                            EventoID = int.Parse(reader["EventoID"].ToString()),
                            DataHora = DateTime.Parse(reader["DataHora"].ToString()),
                            Descricao = reader["Descricao"].ToString()
                        }
                    );
                }
                return View(eventos);
            }
            catch (Exception e)
            {
                return View("IndexError", e);
            }

*/ 
