using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.ConciliacionHorometros
{
    public enum unidadCostoEnum
    
    {
        [DescriptionAttribute("N/A")]
        na = 0,
        [DescriptionAttribute("HORAS")]
        horas = 1,
        [DescriptionAttribute("DIA")]
        dia = 2,
        [DescriptionAttribute("JOR")]
        jor = 3
    }
}
