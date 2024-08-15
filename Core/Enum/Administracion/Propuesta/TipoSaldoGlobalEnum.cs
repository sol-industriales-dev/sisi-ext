using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum TipoSaldoGlobalEnum
    {
        [DescriptionAttribute("Saldo")]
        Saldo = 1,
        [DescriptionAttribute("SaldoTotal")]
        SaldoTotal = 2,
        [DescriptionAttribute("Total")]
        Total = 3,
        [DescriptionAttribute("GranTotal")]
        GranTotal = 4,
        [DescriptionAttribute("Descripcion")]
        Descripcion = 5,
    }
}
