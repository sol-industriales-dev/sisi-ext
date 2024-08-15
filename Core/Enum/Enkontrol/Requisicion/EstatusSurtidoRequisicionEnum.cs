using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Requisicion
{
    public enum EstatusSurtidoRequisicionEnum
    {
        [DescriptionAttribute("Parcial")]
        Parcial = 1,
        [DescriptionAttribute("Sin Surtir")]
        SinSurtir = 2
    }
}
