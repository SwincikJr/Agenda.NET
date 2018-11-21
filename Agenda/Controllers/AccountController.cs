using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Agenda.Models.Abstract;
using Agenda.Models.Concrete;
using System.Text;

namespace Agenda.Controllers
{
    public class AccountController : Controller
    {
        private IUsuarioRepositorio repositorio;

        public AccountController(IUsuarioRepositorio repo)
        {
            repositorio = repo;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string passWord)
        {
            if (email != "" && passWord != "")
            {
                Usuario usuario = repositorio.Usuarios
                    .FirstOrDefault(u => u.Email == email);

                if (usuario != null && usuario.Password == passWord && usuario.Confirmado == true)
                {
                    FormsAuthentication.SetAuthCookie(usuario.Email, false);
                    Session["Usuario"] = usuario.Username;
                    Session["ID"] = usuario.UserID;
                    if (usuario.ImageData != null) Session["Image"] = File(usuario.ImageData, usuario.ImageMimeType);
                    return Redirect(Url.Action("Index", "Home"));
                }
                else
                {
                    if (usuario != null && usuario.Confirmado == false) ModelState.AddModelError("", "Endereço de e-mail ainda não confirmado. Verifique sua caixa de entrada e realize a confirmação.");
                    else ModelState.AddModelError("", "Usuário e/ou senha incorreto(s)");
                    return View();
                }
            }

            else
            {
                ModelState.AddModelError("", "Favor, informar endereço de e-mail e senha.");
                return View();
            }
        }

        [HttpGet]
        public ViewResult Novo()
        {
            return View(new Usuario ());
        }

        [HttpPost]
        public ActionResult Novo(Usuario user, string passWordConfirm)
        {
            if (ModelState.IsValid
                && user.Password == passWordConfirm
                && repositorio.Usuarios.FirstOrDefault(u => u.Email == user.Email) == null)
            {
                user.UserID = 0;
                user.Confirmado = false;
                user.ImageData = null;
                user.ImageMimeType = null;
                repositorio.EditarUsuario(user);
                string uri = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port.ToString();
                EmailProcessor.Process(user, uri);
                TempData["message"] = "Usuário criado com sucesso! Verifique sua caixa de entrada para confirmação do endereço de e-mail.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (user.Password != passWordConfirm) ModelState.AddModelError("", "As senhas informadas não combinam");
                if (repositorio.Usuarios.FirstOrDefault(u => u.Email == user.Email) != null) ModelState.AddModelError("", "O endereço de e-mail " + user.Email + " já possui uma conta vinculada");
                return View(user);
            }
        }

        public ActionResult LoginStart()
        {
            return Redirect("/Account/Login");
        }

        [HttpGet]
        public ActionResult ConfirmarEmail(int userId)
        {
            Usuario usuario = repositorio.Usuarios.FirstOrDefault(u => u.UserID == userId);
            if (usuario != null && usuario.Confirmado == false) return View("ConfirmarEmail", (object)usuario.UserID);
            else return View("TesteFail");
        } 

        [HttpPost]
        public ActionResult ConfirmarEmail(int userId, string email, string passWord)
        {
            if (email != "" && passWord != "")
            {

                Usuario usuario = repositorio.Usuarios.FirstOrDefault(u => u.UserID == userId);

                if (usuario != null
                    && usuario.Email == email
                    && usuario.Password == passWord)
                {
                    usuario.Confirmado = true;
                    repositorio.EditarUsuario(usuario);
                    FormsAuthentication.SetAuthCookie(usuario.Email, false);
                    Session["Usuario"] = usuario.Username;
                    Session["ID"] = usuario.UserID;
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (usuario != null)
                    {
                       ModelState.AddModelError("", "Endereço de e-mail e/ou senha incorreto(s)");
                       return View("ConfirmarEmail", (object)userId);
                    }
                    return View("TesteFail");
                }
            }
            else
            {
                ModelState.AddModelError("", "Informe o endereço de e-mail e a senha.");
                return View("ConfirmarEmail", (object)userId);
            }
        }

        [HttpGet]
        public ActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarSenha(string email)
        {
            if (email != "")
            {
                Usuario usuario = repositorio.Usuarios.FirstOrDefault(u => u.Email == email);
                if (usuario != null)
                {
                    string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    Random r = new Random();
                    string novaSenha = new string(
                        Enumerable.Repeat(caracteres, 10)
                            .Select(s => s[r.Next(s.Length)])
                            .ToArray()
                    );
                    usuario.Password = novaSenha;
                    repositorio.EditarUsuario(usuario);
                    EmailProcessor.Process(usuario, null);
                    TempData["message"] = "Enviamos uma senha provisória para seu e-mail. Verifique sua caixa de entrada.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "O endereço de e-mail informado não possui cadastro conosco!");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Informe um endereço de e-mail.");
                return View();
            }
        }

        [Authorize]
        public FileContentResult GetImage()
        {
            int id = (int)Session["ID"];
            Usuario usuario = repositorio.Usuarios
                .FirstOrDefault(u => u.UserID == id);
            if (usuario != null)
            {
                return File(usuario.ImageData, usuario.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
