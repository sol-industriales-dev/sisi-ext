using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal.Menus
{
    public enum TipoMenuEnum : int
    {
        [DescriptionAttribute("SUBMENU")]
        SUBMENU = 1,
        [DescriptionAttribute("VISTA")]
        VISTA = 2,
        [DescriptionAttribute("EXTERNO")]
        EXTERNO = 3
    }
}
