using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal.Usuario
{
    public enum TipoUsuarioFacultamientoFacturaEnum
    {
        [DescriptionAttribute("ADMINISTRADOR/AUXILIAR DE OBRA")]
        ADMINISTRADOR_AUXILIAR = 1,
        [DescriptionAttribute("GERENTE")]
        GERENTE = 2
    }
}
