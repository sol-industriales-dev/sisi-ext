using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.Maquinaria.Barrenacion
{
    public enum TipoCapturaEnum
    {
        [DescriptionAttribute("Captura normal")]
        Normal = 1,
        [DescriptionAttribute("Mantenimiento/Reparación")]
        Mantenimiento = 2,
        [DescriptionAttribute("Falta de tramo")]
        FaltaDeTramo = 3,
        [DescriptionAttribute("StandBy")]
        StandBy = 4,
        [DescriptionAttribute("Mal clima")]
        Clima = 5,
        [DescriptionAttribute("Falta de combustible")]
        FaltaDeCombustible = 6
    }
}
