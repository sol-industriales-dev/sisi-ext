using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum MetodoEnum
    {
        [DescriptionAttribute("Capacitación")]
        capacitacion = 1,
        [DescriptionAttribute("Adiestramiento")]
        adiestramiento = 2,
        [DescriptionAttribute("Monitoreo")]
        monitoreo = 3,
        [DescriptionAttribute("Otros")]
        otros = 4
    }
}
