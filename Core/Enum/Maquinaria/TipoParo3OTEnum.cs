using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Core.Enum.Maquinaria
{
    public enum TipoParo3OTEnum
    {
        [DescriptionAttribute("StandBy")]
        StandBy = 1,
        [DescriptionAttribute("Trabajando")]
        Trabajando = 2,
        [DescriptionAttribute("Falta Tramo")]
        FaltaTramo = 3,
    }
}
