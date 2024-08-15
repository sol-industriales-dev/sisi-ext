using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class EstadisticaPorcentajeDTO
    {
        public string cursoID { get; set; }
        public string claveCurso { get; set; }
        public string cursoDesc { get; set; }
        public int totalVigentes { get; set; }
        public int totalFaltante { get; set; }
        public int personalAplica { get; set; }
        public decimal porcentaje { get; set; }
        public string porcentajeString { get; set; }
        public bool reglaPersonalAutorizado { get; set; }
        public TematicaCursoEnum tematica { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
    }
}
