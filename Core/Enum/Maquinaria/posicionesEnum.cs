using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum PosicionesEnum
    {
        [DescriptionAttribute("No Aplica")]
        NO_APLICA = 0,
        [DescriptionAttribute("IZQ")]
        IZQ = 1,
        [DescriptionAttribute("DER")]
        DER = 2,
        [DescriptionAttribute("DEL")]
        DLTR = 3,
        [DescriptionAttribute("TRAS")]
        TR = 4,
        [DescriptionAttribute("FR")]
        FR = 5,
        [DescriptionAttribute("DEL IZQ")]
        D_IZQ = 6,
        [DescriptionAttribute("DEL DER")]
        D_DER = 7,
        [DescriptionAttribute("TRAS IZQ")]
        T_IZQ = 8,
        [DescriptionAttribute("TRAS DER")]
        T_DER = 9,
        [DescriptionAttribute("INTER")]
        INTER = 10
    }
}
