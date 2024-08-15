using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Cotizaciones
{
    public enum CotizacionEnum
    {
        [DescriptionAttribute("En cotización")]
        COTIZACION = 1,
        [DescriptionAttribute("Enviada/Revisón")]
        ENTREGA = 2,
        [DescriptionAttribute("Ganado")]
        GANADO = 3,
        [DescriptionAttribute("Perdido")]
        PERDIDO = 4,
        [DescriptionAttribute("Cancelado")]
        CANCELADO = 5
    }
}
