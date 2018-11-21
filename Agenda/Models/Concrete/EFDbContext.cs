using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Agenda.Models.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
    }
}