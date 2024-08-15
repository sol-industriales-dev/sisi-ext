using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoRequerimientoEnum
    {
        [DescriptionAttribute("ÚNICO")]
        UNICO = 1,
        [DescriptionAttribute("RECURRENTE")]
        RECURRENTE = 2
    }
}
