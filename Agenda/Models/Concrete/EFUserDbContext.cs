using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Agenda.Models.Concrete
{
    public class EFUserDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
    }
}