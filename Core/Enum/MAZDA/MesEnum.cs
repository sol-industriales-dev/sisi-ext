using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.MAZDA
{
    public enum MesEnum
    {
        [DescriptionAttribute("ENERO")]
        ENERO = 1,
        [DescriptionAttribute("FEBRERO")]
        FEBRERO = 2,
        [DescriptionAttribute("MARZO")]
        MARZO = 3,
        [DescriptionAttribute("ABRIL")]
        ABRIL = 4,
        [DescriptionAttribute("MAYO")]
        MAYO = 5,
        [DescriptionAttribute("JUNIO")]
        JUNIO = 6,
        [DescriptionAttribute("JULIO")]
        JULIO = 7,
        [DescriptionAttribute("AGOSTO")]
        AGOSTO = 8,
        [DescriptionAttribute("SEPTIEMBRE")]
        SEPTIEMBRE = 9,
        [DescriptionAttribute("OCTUBRE")]
        OCTUBRE = 10,
        [DescriptionAttribute("NOVIEMBRE")]
        NOVIEMBRE = 11,
        [DescriptionAttribute("DICIEMBRE")]
        DICIEMBRE = 12
    }
}
