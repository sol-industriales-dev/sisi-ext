using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum CargaTanqueEnum
    {
        [DescriptionAttribute("Vacío")]
        vacio = 0,
        [DescriptionAttribute("1/4")]
        cuarto = 1,
        [DescriptionAttribute("1/2")]
        medio = 2,
        [DescriptionAttribute("3/4")]
        trescuartos = 3,
        [DescriptionAttribute("lleno")]
        lleno = 4
    }
}
