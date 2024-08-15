using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion.CincoS
{
    public enum PrivilegioEnum
    {
        [DescriptionAttribute("ADMINISTRADOR")]
        Administrador = 1,
        [DescriptionAttribute("AUDITOR")]
        Auditor = 2,
        [DescriptionAttribute("LÍDER")]
        Lider = 3,
        [DescriptionAttribute("VISOR")]
        Visor = 4
    }
}
