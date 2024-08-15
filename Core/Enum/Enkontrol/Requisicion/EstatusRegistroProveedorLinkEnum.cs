using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Requisicion
{
    public enum EstatusRegistroProveedorLinkEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 1,
        [DescriptionAttribute("VENCIDO")]
        VENCIDO = 2,
        [DescriptionAttribute("REALIZADO")]
        REALIZADO = 3,
        [DescriptionAttribute("CANCELADO")]
        CANCELADO = 4
    }
}
