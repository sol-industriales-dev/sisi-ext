using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Inventario
{
    public enum ComparativoEnum
    {
         [DescriptionAttribute("DESACTIVADO")]
        DESACTIVADO = 0,
        [DescriptionAttribute("ACTIVADO")]
        ACTIVADO = 1,
        [DescriptionAttribute("FINALIZADO]")]
        FINALIZADO = 2
    }
}
