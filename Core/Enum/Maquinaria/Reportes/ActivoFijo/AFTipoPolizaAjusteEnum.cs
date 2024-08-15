using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes.ActivoFijo
{
    public enum AFTipoPolizaAjusteEnum
    {
        //Pólizas que ajustan el monto final del activo (cancelaciones, abonos)
        AjusteDeMonto = 1,

        //Varias movimientos en póliza en enkontrol que al sumarlos dan como resultado el monto de un registro de activo en el excel.
        VariasPolizasUnMovimiento = 2,

        //Un movimiento en enkontrol que son varios movimientos en el excel.
        UnaPolizaVariosMovimientos = 3,

        //Hay una póliza de referencia en enkontrol pero no coincide con el monto en el excel, y no se en que pólizas esta el ajuste.
        MontoDiferenteEnPoliza = 4
    }
}
