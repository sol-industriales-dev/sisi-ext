using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.CtrlPptalOficinasCentrales
{
    public enum AgupacionesEnum
    {
        [DescriptionAttribute("MATERIALES")]
        materiales = 1,
        [DescriptionAttribute("COMBUSTIBLE")]
        combustible = 2,
        [DescriptionAttribute("SERVICIOS")]
        servicios = 3,
        [DescriptionAttribute("RENTAS")]
        rentas = 4,
        [DescriptionAttribute("GASTOS DE VIAJE")]
        gastosViaje = 5,
        [DescriptionAttribute("MANTENIMIENTO")]
        mantenimiento = 6,
        [DescriptionAttribute("SERVICIOS ADMINISTRATIVOS")]
        serviciosAdministrativos = 7
    }
}
