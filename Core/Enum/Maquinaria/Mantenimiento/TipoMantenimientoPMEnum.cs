using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Mantenimiento
{
    public enum TipoMantenimientoPMEnum
    {
        [DescriptionAttribute("PM1 250")]
        PM1_250 = 1,
        [DescriptionAttribute("PM2 500")]
        PM2_500 = 2,
        [DescriptionAttribute("PM1 750")]
        PM1_750 = 3,
        [DescriptionAttribute("PM3 1000")]
        PM3_1000 = 4,
        [DescriptionAttribute("PM1 1250")]
        PM1_1250 = 5,
        [DescriptionAttribute("PM2 1500")]
        PM2_1500 = 6,
        [DescriptionAttribute("PM1 1750")]
        PM1_1750 = 7,
        [DescriptionAttribute("PM4 2000")]
        PM4_2000 = 8
    }
}
