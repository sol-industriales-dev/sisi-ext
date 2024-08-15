using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Subcontratistas
{
    public enum TipoUsuariosEnum
    {
        [DescriptionAttribute("ADMINISTRADOR PMO")]
        administradorPMO = 1,
        [DescriptionAttribute("ADMINISTRADOR")]
        administrador = 2,
        [DescriptionAttribute("EVALUADOR")]
        evaluador = 3,
        [DescriptionAttribute("CONSULTA")]
        consulta = 4
    }
}
