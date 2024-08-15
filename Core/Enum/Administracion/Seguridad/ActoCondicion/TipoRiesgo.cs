using System.ComponentModel;

namespace Core.Enum.Administracion.Seguridad.ActoCondicion
{
    public enum TipoRiesgo
    {
        [DescriptionAttribute("Acto")]
        Acto = 1,
        [DescriptionAttribute("Condicion")]
        Condicion = 2
    }
}
