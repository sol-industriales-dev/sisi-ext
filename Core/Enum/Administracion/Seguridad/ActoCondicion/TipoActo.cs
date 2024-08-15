using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.ActoCondicion
{
    public enum TipoActo
    {
        [DescriptionAttribute("Inseguro")]
        Inseguro = 1,
        [DescriptionAttribute("Seguro")]
        Seguro = 2
    }
}
