using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Encuesta.Proveedores
{
    public enum tipoDependenciaProveedor
    {
        [DescriptionAttribute("Requisición")]
        Requisición = 1,
        [DescriptionAttribute("Orden De Compra")]
        OrdenDeCompra = 2,
    }
}
