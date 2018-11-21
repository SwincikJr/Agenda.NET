using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agenda.Models.Concrete
{
    public static class DateCorretor
    {
        public static string CorrigeDia(this DateTime dt)
        {
            if (dt.Day < 10)
            {
                return '0' + dt.Day.ToString();
            }
            return dt.Day.ToString();
        }

        public static string CorrigeMes(this DateTime dt)
        {
            if (dt.Month < 10)
            {
                return '0' + dt.Month.ToString();
            }
            return dt.Month.ToString();
        }

        public static string CorrigeHora(this DateTime dt)
        {
            if (dt.Hour < 10)
            {
                return '0' + dt.Hour.ToString();
            }
            return dt.Hour.ToString();
        }

        public static string CorrigeMinuto(this DateTime dt)
        {
            if (dt.Minute < 10)
            {
                return '0' + dt.Minute.ToString();
            }
            return dt.Minute.ToString();
        }
    }
}