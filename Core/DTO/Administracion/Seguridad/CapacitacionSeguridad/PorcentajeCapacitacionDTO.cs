using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class PorcentajeCapacitacionDTO
    {
        public ClasificacionCursoEnum clasificacion { get; set; }
        public string clasificacionDesc { get; set; }
        public int cursosAplican { get; set; }
        public int cursosVigentes { get; set; }
        public string porcentajeCapacitacion { get; set; }
    }
}
