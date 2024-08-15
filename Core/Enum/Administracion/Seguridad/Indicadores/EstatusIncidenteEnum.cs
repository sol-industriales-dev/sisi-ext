using System.ComponentModel;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum EstatusIncidenteEnum
    {
        [DescriptionAttribute("Pendiente Cargar IP")]
        PendienteCargaIP = 1,
        [DescriptionAttribute("Pendiente Generar RIA")]
        PendienteGeneracionRIA = 2,
        [DescriptionAttribute("Pendiente Cargar RIA")]
        PendienteCargaRIA = 3,
        [DescriptionAttribute("Completo")]
        Completo = 4
    }
}
