using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum TipoConcentradoEnum
    {
        [DescriptionAttribute("Saldo Conciliado")]
        SaldoConciliado = 0,
        [DescriptionAttribute("Cadena Productiva")]
        CadenaProductiva = 1,
        [DescriptionAttribute("Reserva")]
        Reserva = 2,
        [DescriptionAttribute("Cheque")]
        Cheques = 3,
        [DescriptionAttribute("Anticipos")]
        Anticipos = 4,
        [DescriptionAttribute("Intereses por Factoraje")]
        InteresesFactoraje = 5,
        [DescriptionAttribute("Polizas de Diario")]
        PolizasDiario = 6,
        [DescriptionAttribute("Estado de Cuenta")]
        EstadoCuenta = 7,
        [DescriptionAttribute("Movimientos de Clientes")]
        MovimientoCliente = 8,
        [DescriptionAttribute("Nomina")]
        Nomina = 9,
    }
}
