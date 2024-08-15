using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion.CincoS
{
    public enum RespuestaEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 0,
        [DescriptionAttribute("OK")]
        OK = 1,
        [DescriptionAttribute("NO OK")]
        NO_OK = 2
    }
}
