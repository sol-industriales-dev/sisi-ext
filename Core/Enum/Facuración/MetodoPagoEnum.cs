using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Facuración
{
    public enum MetodoPagoEnum
    {
        [DescriptionAttribute("TRANSFERENCIA BANCARIA")]
        TRANSFERENCIA_BANCARIA = 1,
        [DescriptionAttribute("CHEQUE NOMINATIVO")]
        CHEQUE_NOMINATIVO = 2,
        [DescriptionAttribute("NA")]
        NA = 3,
        [DescriptionAttribute("OTROS")]
        OTROS = 4,
        [DescriptionAttribute("TARJETA DEBITO")]
        TARJETA_DEBITO = 5,
        [DescriptionAttribute("EFECTIVO")]
        EFECTIVO = 6
    }
}
