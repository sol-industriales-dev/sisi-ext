using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum tipoPropuestaEnum
    {
        [DescriptionAttribute("SinSaldo")]
        SinSaldo = 0,
        [DescriptionAttribute("Saldo")]
        Saldo = 1,
        [DescriptionAttribute("Suma")]
        Suma = 2,
        [DescriptionAttribute("Encabezado")]
        Encabezado = 3,
        [DescriptionAttribute("SaldoEncabezado")]
        SaldoEncabezado = 4,
        [DescriptionAttribute("Input")]
        Input = 10,
        [DescriptionAttribute("InputEncabezado")]
        InputEncabezado = 11,
    }
}
