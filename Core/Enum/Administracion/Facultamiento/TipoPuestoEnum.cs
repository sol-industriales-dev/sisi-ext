using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Facultamiento
{
    public enum TipoPuestoEnum
    {
        [DescriptionAttribute("Servicios Administrativos")]
        ServAdmin = 1,
        [DescriptionAttribute("Seguros")]
        Seguros = 2,
        [DescriptionAttribute("Equipo Cómputo")]
        EqComputo = 3,
    }
}
