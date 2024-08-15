using System.ComponentModel;


namespace Core.Enum.Administracion.Seguridad.CapacitacionSeguridad
{
    public enum TipoExamenEnum
    {
        [DescriptionAttribute("Examen diagnóstico")]
        Diagnostico = 1,
        [DescriptionAttribute("Examen final")]
        Final = 2
    }
}
