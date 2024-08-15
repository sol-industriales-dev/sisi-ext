using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras
{
    public enum PERU_TipoInsumoENUM
    {
        [DescriptionAttribute("INVENTARIABLE")]
        INVENTARIABLE = 1,
        [DescriptionAttribute("SERVICIO")]
        SERVICIO = 2
    }
}
