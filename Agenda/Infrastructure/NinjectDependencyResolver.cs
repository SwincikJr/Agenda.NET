using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Mvc;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System.Configuration;
using Agenda.Models.Abstract;
using Agenda.Models.Concrete;
using Moq;
using System.Linq;

namespace Agenda.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IEventoRepositorio>().To<EFEventoRepositorio>();
            kernel.Bind<IUsuarioRepositorio>().To<EFUsuarioRepositorio>();
        }
    }
}