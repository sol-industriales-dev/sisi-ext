using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad
{
    public enum ComentarioEnum
    {
        [DescriptionAttribute("NA")]
        NA = 0,
        [DescriptionAttribute("OK")]
        OK = 1,
        [DescriptionAttribute("NOK")]
        NOK = 2,
        [DescriptionAttribute("Regular")]
        Regular = 3,
        [DescriptionAttribute("Corregido")]
        Corregido = 4,
    }
}
