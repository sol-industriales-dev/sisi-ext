using System.ComponentModel;

namespace Core.Enum.Administracion.Comercializacion.CRM
{
    public enum MenuEnum
    {
        [DescriptionAttribute("Dashboard")]
        DASHBOARD = 1,
        [DescriptionAttribute("Clientes")]
        CLIENTES = 2,
        [DescriptionAttribute("Prospectos")]
        PROSPECTOS = 3,
        [DescriptionAttribute("Canales")]
        CANALES = 4,
        [DescriptionAttribute("Tracking ventas")]
        TRACKING_VENTAS = 5,
        [DescriptionAttribute("Usuarios CRM")]
        USUARIOS_CRM = 6
    }
}