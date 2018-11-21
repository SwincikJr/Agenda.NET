using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agenda.Models.Concrete;

namespace Agenda.Models.Abstract
{
    public interface IUsuarioRepositorio
    {
        IQueryable<Usuario> Usuarios { get; }

        void EditarUsuario(Usuario usuario);
    }
}