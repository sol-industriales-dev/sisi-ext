using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras
{
    public enum TipoImporte
    {
        [DescriptionAttribute("Orden de compra")]
        ordenCompra = 1,
        [DescriptionAttribute("Cancelada")]
        cancelada = 2,
        [DescriptionAttribute("rec?")]
        rec = 3,
        [DescriptionAttribute("Ajustada")]
        ajustada = 4,
        [DescriptionAttribute("Factura")]
        factura = 5,
        [DescriptionAttribute("Pago")]
        pago = 6,
        [DescriptionAttribute("Retenido")]
        retenido = 7,
        [DescriptionAttribute("Anticipo ")]
        anticipo = 8,
    }
}
