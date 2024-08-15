using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad
{
    public enum TipoLicenciaEnum
    {
        [DescriptionAttribute("A")]
        A = 1,
        [DescriptionAttribute("B")]
        B = 2,
        [DescriptionAttribute("C")]
        C = 3,
        [DescriptionAttribute("D")]
        D = 4,
    }
}
