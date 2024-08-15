using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum TipoCombustibleEnum
    {
        [DescriptionAttribute("No Aplica")]
        NOAPLICA = 0,
        [DescriptionAttribute("Gasolina")]
        GASOLINA = 1,
        [DescriptionAttribute("Diesel")]
        DIESEL = 3
    }
}
