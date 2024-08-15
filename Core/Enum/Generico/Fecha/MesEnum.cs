using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Generico.Fecha
{
    public enum MesEnum
    {
        [DescriptionAttribute("Enero")]
        Enero = 1,
        [DescriptionAttribute("Febrero")]
        Febrero = 2,
        [DescriptionAttribute("Marzo")]
        Marzo = 3,
        [DescriptionAttribute("Abril")]
        Abril = 4,
        [DescriptionAttribute("Mayo")]
        Mayo = 5,
        [DescriptionAttribute("Junio")]
        Junio = 6,
        [DescriptionAttribute("Julio")]
        Julio = 7,
        [DescriptionAttribute("Agosto")]
        Agosto = 8,
        [DescriptionAttribute("Septiembre")]
        Septiembre = 9,
        [DescriptionAttribute("Octubre")]
        Octubre = 10,
        [DescriptionAttribute("Noviembre")]
        Noviembre = 11,
        [DescriptionAttribute("Diciembre")]
        Diciembre = 12
    }
}
