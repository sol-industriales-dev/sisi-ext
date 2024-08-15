using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum PrivilegioOrdenCambioEnum
    {
        [DescriptionAttribute("ADMINISTRADOR")]
        ADMINISTRADOR = 1,
        [DescriptionAttribute("GESTOR OC")]
        GESTOR_OC = 2,
        [DescriptionAttribute("VISOR")]
        VISOR = 3,
        [DescriptionAttribute("INTERESADO")]
        INTERESADO = 4
    }
}
