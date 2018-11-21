using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agenda.Models.Concrete;
using Agenda.Models.Abstract;

namespace Agenda.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private IEventoRepositorio repositorio;
        public EventoController(IEventoRepositorio repo)
        {
            this.repositorio = repo;
        }

        [HttpGet]
        public ViewResult Novo()
        {
            Evento novoEvento = new Evento
            {
                EventoID = 0,
                UserID = (int)Session["ID"],
                dataHora = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, 0, 0),
                Descricao = ""
            };
            return View(novoEvento);
        }

        [HttpPost]
        public ActionResult Novo(Evento model, string beginDate, string beginTime)
        {
            if (model != null && model.UserID == (int)Session["ID"])
            {
                model.dataHora = new DateTime(
                    DateTime.Parse(beginDate).Year,
                    DateTime.Parse(beginDate).Month,
                    DateTime.Parse(beginDate).Day,
                    DateTime.Parse(beginTime).Hour,
                    DateTime.Parse(beginTime).Minute,
                    0);
                if (model.dataHora > DateTime.Now)
                {
                    model.EventoID = 0;
                    repositorio.EditarEvento(model);
                    TempData["EventoModificado"] = "Novo evento para o dia " + model.dataHora.Date.ToString("dd/MM/yyy") + " criado com sucesso!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Um evento não pode ser agendado para uma data passada.");
                    return View(model);
                }
            }
            else
            {
                return View("Fail");
            }
        }

        [HttpGet]
        public ViewResult Editar(int eventoId)
        {
            Evento evento = repositorio.Eventos
                .FirstOrDefault(e => e.EventoID == eventoId);

            if (evento != null && evento.UserID == (int)Session["ID"]) return View(evento);
            else return View("Fail");
        }

        [HttpPost]
        public ActionResult Editar(Evento model, string beginDate, string beginTime)
        {
            if (model != null && model.UserID == (int)Session["ID"])
            {
                model.dataHora = new DateTime(
                    DateTime.Parse(beginDate).Year,
                    DateTime.Parse(beginDate).Month,
                    DateTime.Parse(beginDate).Day,
                    DateTime.Parse(beginTime).Hour,
                    DateTime.Parse(beginTime).Minute,
                    0);
                if (model.dataHora > DateTime.Now)
                {
                    repositorio.EditarEvento(model);
                    TempData["EventoModificado"] = "Evento modificado com sucesso para " + model.dataHora.Date.ToString("dd/MM/yyy") + " às " + model.dataHora.ToShortTimeString() + ".";
                    if (Session["CurrentPage"].ToString() == "Index") return RedirectToAction("Index", "Home");
                    else return Redirect("/Evento/Consultar/" + Session["beginDate"]);
                }
                else
                {
                    ModelState.AddModelError("", "Um evento não pode ser agendado para uma data passada.");
                    return View(model);
                }
            }
            else
            {
                return View("Fail");
            }
        }

        [HttpPost]
        public ActionResult Deletar(int eventoId)
        {
            Evento evento = repositorio.Eventos.FirstOrDefault(e => e.EventoID == eventoId);
            if (evento != null && evento.UserID == (int)Session["ID"])
            {
                repositorio.DeletarEvento(eventoId);
                TempData["EventoModificado"] = "Evento deletado com sucesso!";
                if (Session["CurrentPage"].ToString() == "Index") return RedirectToAction("Index", "Home");
                else return Redirect("/Evento/Consultar/" + Session["beginDate"]);
            }
            else return View("Fail");
        }

        [HttpGet]
        public ViewResult Consultar(string beginDate)
        {
            List<Evento> eventos = new List<Evento>();
            foreach (Evento e in repositorio.Eventos)
            {
                if (e.UserID == (int)Session["ID"] && e.dataHora.Date.ToString("yyyy-MM-dd") == beginDate)
                {
                    eventos.Add(e);
                }
            }
            IEnumerable<Evento> eventosOrdenados = eventos
                .OrderBy(e => (e.dataHora.Hour * 60 * 60 + e.dataHora.Minute * 60 + e.dataHora.Second));

            Session["CurrentPage"] = "Consultar";
            Session["beginDate"] = beginDate;
            return View(eventosOrdenados);
        }

        [HttpPost]
        public ActionResult Post(string beginDate)
        {
            return Redirect("/Evento/Consultar/" + beginDate);
        }
    }
}
