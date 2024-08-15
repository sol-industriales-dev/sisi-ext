using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.StandBy
{
    public enum AccionActivacionEconomicoEnum
    {
        ELABORACION_REQUISICION = 1,
        ELABORACION_ORDEN_COMPRA = 2,
        CAPTURA_HOROMETROS = 3,
        CAPTURA_COMBUSTIBLE = 4,
        CAPTURA_ACEITE = 5,
        RECEPCION_FACTURA = 6,
        SALIDA_ALMACEN = 7
    }
}
