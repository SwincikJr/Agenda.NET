using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Agenda
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Confirmar Email", "Account/ConfirmarEmail/{userId}",
                new { controller = "Account", action = "ConfirmarEmail" },
                new { userId = @"\d+" });

            routes.MapRoute("Consultar", "Evento/Consultar/{beginDate}",
                new { controller = "Evento", action = "Consultar" });

            routes.MapRoute("EditarEvento", "Evento/Editar/{eventoId}",
                new { controller = "Evento", action = "Editar" },
                new { eventoId = @"\d+" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}