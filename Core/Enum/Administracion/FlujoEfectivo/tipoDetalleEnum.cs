using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum tipoDetalleEnum
    {
        [DescriptionAttribute("Cuenta")]
        ConsultaCuenta = 0,
        [DescriptionAttribute("Poliza")]
        ConsultaPoliza = 1,
        [DescriptionAttribute("Concepto")]
        ConsultaConcepto = 2,
        [DescriptionAttribute("Cuenta")]
        FlujoTotalCuenta = 3,
        [DescriptionAttribute("Concepto")]
        FlujoTotalConcepto = 4,
        [DescriptionAttribute("Obra")]
        ConsultaCentroCostos = 5,
        [DescriptionAttribute("Obra")]
        FlujoTotalCentroCostos = 6,
        [DescriptionAttribute("Principal")]
        CierrePrincipal = 7,
        [DescriptionAttribute("Manual")]
        CierreManual = 8,
        [DescriptionAttribute("Reserva")]
        CierreReserva = 9,
        [DescriptionAttribute("Concepto")]
        CierreConcepto = 10,
        [DescriptionAttribute("Factura")]
        CierreFactura = 11,
        [DescriptionAttribute("Principal")]
        CierrePrincipalArrendadora = 12,
        [DescriptionAttribute("Manual")]
        CierreManualArrendadora = 13,
        [DescriptionAttribute("Reserva")]
        CierreReservaArrendadora = 14,
        [DescriptionAttribute("Concepto")]
        CierreConceptoArrendadora = 15,
        [DescriptionAttribute("Factura")]
        CierreFacturaArrendadora = 16,
        [DescriptionAttribute("Categoría")]
        PlanPrincipal = 17,
        [DescriptionAttribute("Proveedor")]
        PlanProveedor = 18,
        [DescriptionAttribute("Cliente")]
        PlanCliente = 19,
        [DescriptionAttribute("Manual")]
        PlanManual = 20,
        [DescriptionAttribute("Factura")]
        PlanProveedorFactura = 21,
        [DescriptionAttribute("Factura")]
        PlanClienteFactura = 22,
        [DescriptionAttribute("Empresa")]
        ConsultaEmpresa = 23,
        [DescriptionAttribute("Empresa")]
        FlujoEmpresa = 24,
        [DescriptionAttribute("Empresa")]
        PlanEmpresa = 25,
        [DescriptionAttribute("Empresa")]
        CierreEmpresa = 26,
        [DescriptionAttribute("Programacion Contratos")]
        contratos = 27
    }
}
