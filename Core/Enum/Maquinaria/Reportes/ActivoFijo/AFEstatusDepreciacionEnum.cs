using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes.ActivoFijo
{
    public enum AFEstatusDepreciacionEnum
    {
        [DescriptionAttribute("DEPRECIADAS")]
        Depreciadas = 0,
        [DescriptionAttribute("DEPRECIANDO")]
        Depreciando = 1,
        [DescriptionAttribute("BAJAS")]
        Bajas = 2
    }
}