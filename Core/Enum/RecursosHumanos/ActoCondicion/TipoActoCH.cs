using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.ActoCondicion
{
    public enum TipoActoCH
    {
        [DescriptionAttribute("Inseguro")]
        Inseguro = 1,
        [DescriptionAttribute("Seguro")]
        Seguro = 2
    }
}
