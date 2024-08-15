using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos
{
    public enum Tipo_NominaEnum
    {
        [DescriptionAttribute("SEM")]
        SEMANAL = 1,
        //[DescriptionAttribute("Decenal")]
        //Decenal = 2,
        //[DescriptionAttribute("Cartocena")]
        //Cartocena = 3,
        [DescriptionAttribute("QUIN")]
        QUINCENAL = 4
        //[DescriptionAttribute("Mensual")]
        //Mensual = 5,
    }
}
