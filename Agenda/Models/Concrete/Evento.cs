using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agenda.Models.Concrete
{
    public class Evento
    {
        public int EventoID { get; set; }
        public int UserID { get; set; }
        public DateTime dataHora { get; set; }
        public string Descricao { get; set; }
    }
}