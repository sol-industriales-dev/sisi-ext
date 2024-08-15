using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Almacen
{
    public enum TipoAlmacenEnum
    {
        [DescriptionAttribute("COMPONENTES")]
        COMPONENTES = 1,
        [DescriptionAttribute("ACEITES")]
        ACEITES = 2,
        [DescriptionAttribute("PRINCIPALES")]
        PRINCIPALES = 3,
        [DescriptionAttribute("OTROS")]
        OTROS = 4
    }
}
