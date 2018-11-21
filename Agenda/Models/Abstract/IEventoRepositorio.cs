using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agenda.Models.Concrete;

namespace Agenda.Models.Abstract
{
    public interface IEventoRepositorio
    {
        IQueryable<Evento> Eventos { get; }

        void EditarEvento(Evento evento);

        Evento DeletarEvento(int eventoId);
    }

}