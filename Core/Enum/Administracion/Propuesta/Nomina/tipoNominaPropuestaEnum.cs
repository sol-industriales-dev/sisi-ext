using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta.Nomina
{
    public enum tipoNominaPropuestaEnum
    {
        [DescriptionAttribute("NO ASIGNAR")]
        NA = 0,
        [DescriptionAttribute("NOMINA SEMANAL")]
        Semanal = 1,
        //[DescriptionAttribute("Decenal")]
        //Decenal = 2,
        //[DescriptionAttribute("Cartocena")]
        //Cartocena = 3,
        [DescriptionAttribute("NOMINA QUINCENAL")]
        Quincenal = 4,
        //[DescriptionAttribute("Mensual")]
        //Mensual = 5,
        [DescriptionAttribute("PRESTAMO")]
        Prestamo = 6,
        [DescriptionAttribute("FINIQUITO")]
        Finiquito = 7,
        [DescriptionAttribute("IMSS")]
        Imss = 8,
        [DescriptionAttribute("BONO")]
        Bono = 9,
        [DescriptionAttribute("AGUINALDO")]
        Aguinaldo = 10,
        [DescriptionAttribute("ISR")]
        ISR = 11,
        [DescriptionAttribute("ISN ")]
        ISN = 12,
        [DescriptionAttribute("LIQUIDACION SEMANAL")]
        LIQUIDACION_SEMANAL = 13,
        [DescriptionAttribute("LIQUIDACION QUINCENAL")]
        LIQUIDACION_QUINCENAL = 14,
        [DescriptionAttribute("REGIMEN OBRERO")]
        OBRERO = 20,
        [DescriptionAttribute("REGIMEN EMPLEADO")]
        EMPLEADO = 21,
        [DescriptionAttribute("REGIMEN CONSTRUCCION CIVIL")]
        CONTRUCCION_CIVIL = 27,
    }
}
