using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agenda.Models.Abstract;

namespace Agenda.Models.Concrete
{
    public class EFUsuarioRepositorio : IUsuarioRepositorio
    {
        private EFUserDbContext context = new EFUserDbContext();

        public IQueryable<Usuario> Usuarios
        {
            get { return context.Usuarios; }
        }

        public void EditarUsuario(Usuario usuario)
        {
            if (usuario.UserID == 0) context.Usuarios.Add(usuario);
            else
            {
                Usuario dbEntry = context.Usuarios.Find(usuario.UserID);
                if (dbEntry != null)
                {
                    dbEntry.Email = usuario.Email;
                    dbEntry.Username = usuario.Username;
                    dbEntry.Password = usuario.Password;
                    dbEntry.Confirmado = usuario.Confirmado;
                    dbEntry.ImageData = usuario.ImageData;
                    dbEntry.ImageMimeType = usuario.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
    }
}