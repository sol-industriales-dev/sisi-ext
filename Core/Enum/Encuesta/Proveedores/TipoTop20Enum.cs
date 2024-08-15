using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Encuesta.Proveedores
{
    public enum TipoTop20Enum
    {
        [DescriptionAttribute("C")]
        Cantidad = 1,
        [DescriptionAttribute("M")]
        Monto = 2,
        [DescriptionAttribute("N")]
        Nuevo = 3
    }
}