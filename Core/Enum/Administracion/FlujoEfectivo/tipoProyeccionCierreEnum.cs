using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum tipoProyeccionCierreEnum
    {
        [DescriptionAttribute("Manual")]
        Manual = 0,
        [DescriptionAttribute("Facturas de clientes")]
        FacturasClientes = 1,
        [DescriptionAttribute("Rentenciones de clientes")]
        RetencionesClientes = 2,
        [DescriptionAttribute("Movimiento de Proveedor")]
        MovimientoProveedor = 3,
        [DescriptionAttribute("Cadena Productiva")]
        CadenaProductiva = 4,
        [DescriptionAttribute("Amortización de Clientes")]
        AmortizacionClientes = 5,
        [DescriptionAttribute("Movimiento de Arrendadora")]
        MovimientoArrendadora = 6,
        [DescriptionAttribute("Anticipo de clientes")]
        AnticipoClientes = 7,
        [DescriptionAttribute("Anticipo de contratistas")]
        AnticipoContratista = 8,
        [DescriptionAttribute("Documentos Por Pagar")]
        DocPorPagar = 9,
        [DescriptionAttribute("Kubrix Ingresos")]
        KubrixIngresos = 10,
        [DescriptionAttribute("Kubrix Costos")]
        KubrixCostos = 11,
        [DescriptionAttribute("Retencion Contratistas")]
        RetencionContratistas = 12,
    }
}
