using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum Tipo_EncierroEnum
    {
        [DescriptionAttribute("Libre")]
        LIBRE = 1,
        [DescriptionAttribute("Fin de Semana")]
        FIN_SEMANA = 2,
        [DescriptionAttribute("Diario")]
        DIARIO = 3,
        [DescriptionAttribute("No Aplica")]
        NOAPLICA = 4
    } 

}
