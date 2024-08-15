using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Demandas
{
    public enum EstatusDemandaEnum
    {
        [DescriptionAttribute("Cerrado")]
        CERRADO = 1,
        [DescriptionAttribute("Abierto")]
        ABIERTO = 2
    }
}
