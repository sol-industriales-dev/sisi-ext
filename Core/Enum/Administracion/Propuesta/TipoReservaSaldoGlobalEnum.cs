using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum TipoReservaSaldoGlobalEnum
    {
        [DescriptionAttribute("NoAsignado")]
        NoAsignado = 0,
        [DescriptionAttribute("Anticipos")]
        Anticipos = 1,
        [DescriptionAttribute("Otros")]
        Otros = 2,
        [DescriptionAttribute("Reservas")]
        Reservas = 3,
        [DescriptionAttribute("Impuestos")]
        Impuestos = 4,
    }
}
