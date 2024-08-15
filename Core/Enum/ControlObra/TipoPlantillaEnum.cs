using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoPlantillaEnum
    {
        [DescriptionAttribute("PERIÓDICO")]
        PERIODICO = 1,
        [DescriptionAttribute("ARRANQUE")]
        ARRANQUE = 2,
        [DescriptionAttribute("CIERRE")]
        CIERRE = 3
    }
}
