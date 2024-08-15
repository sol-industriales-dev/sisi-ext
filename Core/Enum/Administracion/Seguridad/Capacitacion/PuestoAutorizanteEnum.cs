using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum PuestoAutorizanteEnum
    {
        [DescriptionAttribute("Coordinador CMCAP")]
        CoordinadorCMCAP = 1,
        [DescriptionAttribute("Coordinador de CSH")]
        CoordinadorCSH = 2,
        [DescriptionAttribute("Secretario de CSH")]
        SecretarioCSH = 3,
        [DescriptionAttribute("Gerente de Proyecto")]
        GerenteProyecto = 4,
    }
}
