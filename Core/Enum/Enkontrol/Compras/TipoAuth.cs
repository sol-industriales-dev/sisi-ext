using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras
{
    public enum TipoAuth
    {
        [DescriptionAttribute("En espera")]
        enEspera = 0,
        [DescriptionAttribute("Acepta")]
        acepta = 1,
        [DescriptionAttribute("rechaza")]
        rechaza = 2,
    }
}
