using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum EstatusAutorizacionCapacitacion
    {
        [DescriptionAttribute("Pendiente")]
        Pendiente = 1,
        [DescriptionAttribute("Autorizado")]
        Autorizado = 2,
        [DescriptionAttribute("Rechazado")]
        Rechazado = 3
    }
}
