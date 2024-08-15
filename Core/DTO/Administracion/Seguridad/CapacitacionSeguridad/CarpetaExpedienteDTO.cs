using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class CarpetaExpedienteDTO
    {
        public int year { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
        public string nombreControlAsistencia { get; set; }
        public string rutaListaAsistencia { get; set; }
        public string rutaListaAutorizacion { get; set; }
        public string rutaExamenInicial { get; set; }
        public string rutaExamenFinal { get; set; }
    }
}
