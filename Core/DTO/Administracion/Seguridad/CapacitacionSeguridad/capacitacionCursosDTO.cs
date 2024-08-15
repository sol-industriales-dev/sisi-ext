using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class capacitacionCursosDTO
    {
        public int id { get; set; }
        public string claveCurso { get; set; }
        public string nombre { get; set; }
        public decimal duracion { get; set; }
        public string objetivo { get; set; }
        public string temasPrincipales { get; set; }
        public string referenciasNormativas { get; set; }
        public string estatus { get; set; }
        public int esGeneral { get; set; }
        public bool aplicaTodosPuestos{ get; set; }
        public bool capacitacionUnica { get; set; }
        public string nota { get; set; }
        public string clasificacionDesc { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }

        public List<string> nombreExamen { get; set; }
        public List<mandosCursoDTO> mandos { get; set; }
        public List<puestosCursoDTO> puestos { get; set; }
        public List<puestosCursoDTO> puestosAutorizacion { get; set; }
        public List<centrosCostoCursoDTO> centrosCosto { get; set; }
    }
}
