using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class EstadisticaPorcentaje2DTO
    {
        public string cursoID { get; set; }
        public string claveCurso { get; set; }
        public string cursoDesc { get; set; }
        public int totalVigentes { get; set; }
        public int totalFaltante { get; set; }
        public int personalAplica { get; set; }
        public string porcentaje { get; set; }
        public decimal porcentajeNumero { get; set; }
        public string porcentajeString { get; set; }
        public bool reglaPersonalAutorizado { get; set; }
        public TematicaCursoEnum tematica { get; set; }
        public ClasificacionCursoEnum clasificacionEnum { get; set; }
        public string clasificacion { get; set; }
    }
}
