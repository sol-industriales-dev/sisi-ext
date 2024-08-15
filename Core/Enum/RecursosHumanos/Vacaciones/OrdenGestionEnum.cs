using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Vacaciones
{
    public enum OrdenGestionEnum
    {
        [DescriptionAttribute("JEFE INMEDIATO")]
        JEFE_INMEDIATO = 0,
        [DescriptionAttribute("RESPONSABLE CC")]
        RESPONSABLE_CC = 1,
        [DescriptionAttribute("AUTORIZANTE PAGADAS 1")]
        AUTORIZANTE_PAGADAS_1 = 2,
        [DescriptionAttribute("AUTORIZANTE PAGADAS 2")]
        AUTORIZANTE_PAGADAS_2 = 3,
        [DescriptionAttribute("Medico")]
        MEDICO = 4,
        [DescriptionAttribute("CAPITAL HUMANO")]
        CAPITAL_HUMANO = 5
    }
}
