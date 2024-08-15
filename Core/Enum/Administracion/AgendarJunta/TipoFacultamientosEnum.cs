using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.AgendarJunta
{
    public enum TipoFacultamientosEnum
    {
        [DescriptionAttribute("Administrador")]
        ADMINISTRADOR = 1,
        [DescriptionAttribute("Usuario")]
        USUARIO = 2
    }
}
