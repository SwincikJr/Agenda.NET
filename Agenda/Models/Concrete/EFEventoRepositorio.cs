using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Agenda.Models.Abstract;

namespace Agenda.Models.Concrete
{
    public class EFEventoRepositorio : IEventoRepositorio
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Evento> Eventos
        {
            get { return context.Eventos; }
        }

        //Alteração no banco de dados
        public void EditarEvento(Evento evento)
        {
            if (evento.EventoID == 0)
            {
                context.Eventos.Add(evento);
            }
            else
            {
                Evento dbEntry = context.Eventos.Find(evento.EventoID);
                if (dbEntry != null)
                {
                    dbEntry.dataHora = evento.dataHora;
                    dbEntry.Descricao = evento.Descricao;
                }
            }
            context.SaveChanges();
        }

        public Evento DeletarEvento(int eventoId)
        {
            Evento dbEntry = context.Eventos.Find(eventoId);
            if (dbEntry != null)
            {
                context.Eventos.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}