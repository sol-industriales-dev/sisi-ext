using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes.CuadroComparativo.Equipo
{
    public enum AdquisicionEnum
    {
        [DescriptionAttribute("Compra")]
        Compra = 1,
        [DescriptionAttribute("Renta")]
        Renta = 2,
        [DescriptionAttribute("Roc")] //Equipo Propio
        Roc = 3,
    }
}
