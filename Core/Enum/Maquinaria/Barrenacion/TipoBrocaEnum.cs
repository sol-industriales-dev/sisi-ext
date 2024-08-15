using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.Maquinaria.Barrenacion
{
    public enum TipoBrocaEnum
    {
        [DescriptionAttribute("No Aplica")]
        NO_APLICA = 0,
        [DescriptionAttribute("7 1/2")]
        SIETEYMEDIO = 1,
        [DescriptionAttribute("6 1/2")]
        SEISYMEDIO = 2,
        [DescriptionAttribute("5 1/2")]
        CINCOYMEDIO = 3,
        [DescriptionAttribute("5 3/4")]
        CINCOYTRESCUARTOS = 4,
        [DescriptionAttribute("6 3/4")]
        SEISYTRESCUARTOS = 5,
        [DescriptionAttribute("8")]
        OCHO = 6
    }
}
