using System.ComponentModel;

namespace Core.Enum.Administracion.Comercializacion.CRM
{
    public enum PrioridadesEnum
    {
        [DescriptionAttribute("Prospección")]
        PROSPECCION = 1,
        [DescriptionAttribute("Labor de venta")]
        LABOR_DE_VENTA = 2,
        [DescriptionAttribute("Cotización")]
        COTIZACION = 3,
        [DescriptionAttribute("Negociación")]
        NEGOCIACION = 4,
        [DescriptionAttribute("Cierre")]
        CIERRE = 5
    }
}