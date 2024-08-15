using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Facuración
{
    public enum PrefacturaciónEnum
    {
        [DescriptionAttribute("EnEspera")]
        EnEspera = 1,
        [DescriptionAttribute("Aceptado")]
        Aceptado = 2,
        [DescriptionAttribute("Rechzado")]
        Rechzado = 3
    }
}
