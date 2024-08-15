using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
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
