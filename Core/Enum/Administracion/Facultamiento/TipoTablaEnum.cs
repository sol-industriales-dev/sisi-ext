using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Facultamiento
{
    public enum TipoTablaEnum
    {
        [DescriptionAttribute("Refacciones")]
        Refacciones = 1,
        [DescriptionAttribute("Materiales")]
        Materiales = 2,
        [DescriptionAttribute("Administrativo")]
        Administrativo = 3,
    }
}
