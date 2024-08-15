using System.ComponentModel;


namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum TipoExamenEnum
    {
        [DescriptionAttribute("Examen diagnóstico")]
        Diagnostico = 1,
        [DescriptionAttribute("Examen final")]
        Final = 2
    }
}
