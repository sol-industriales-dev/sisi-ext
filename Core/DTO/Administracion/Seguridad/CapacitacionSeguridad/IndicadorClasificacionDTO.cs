using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class IndicadorClasificacionDTO
    {
        public ClasificacionCursoEnum clasificacion { get; set; }
        public bool capacitado { get; set; }
        public string cursoDesc { get; set; }
        public string cursoClave { get; set; }
        public string cursoID { get; set; }
        public List<int> listaMandos { get; set; }
        public string departamentoDesc { get; set; }
        public string departamentoID { get; set; }
        public string ccDesc { get; set; }
        public TematicaCursoEnum tematica { get; set; }
    }
}
