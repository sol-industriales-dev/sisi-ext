using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol
{
    public enum TipoUltimaAccionEnum
    {
        [DescriptionAttribute("Autorización")]
        Autorizacion = 1,
        [DescriptionAttribute("Desautorización")]
        Desautorizacion = 2,
        [DescriptionAttribute("Cancelación")]
        Cancelacion = 3,
        [DescriptionAttribute("Eliminación")]
        Eliminacion = 4
    }
}
