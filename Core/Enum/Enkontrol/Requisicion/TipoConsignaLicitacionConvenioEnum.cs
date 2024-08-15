using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Requisicion
{
    public enum TipoConsignaLicitacionConvenioEnum
    {
        [DescriptionAttribute("Consigna")]
        CONSIGNA = 1,
        [DescriptionAttribute("Licitación")]
        LICITACION = 2,
        [DescriptionAttribute("CRC")]
        CRC = 3,
        [DescriptionAttribute("Convenio")]
        CONVENIO = 4
    }
}
