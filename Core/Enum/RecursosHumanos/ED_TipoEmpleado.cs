using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos
{
    public enum ED_TipoEmpleado
    {
        [DescriptionAttribute("Administrador")]
        ADMIN = 1,
        [DescriptionAttribute("Jefe")]
        JEFE = 2,
        [DescriptionAttribute("Subordinado")]
        SUBOR = 3
    }
}
