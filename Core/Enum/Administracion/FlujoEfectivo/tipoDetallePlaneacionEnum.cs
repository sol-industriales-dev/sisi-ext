

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum tipoDetallePlaneacionEnum : int
    {
        [DescriptionAttribute("Manual")]
        manual = 1,
        [DescriptionAttribute("Cadena Productiva")]
        cadenaProductiva = 2,
        [DescriptionAttribute("Nomina")]
        nomina = 3,
        [DescriptionAttribute("Efectivo Recibido")]
        efectivoRecibido = 4,
        [DescriptionAttribute("Gastos Proyecto")]
        gastosProyecto = 5,
        [DescriptionAttribute("DOC X PAGAR")]
        contratos = 6
    }
}
