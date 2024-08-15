using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionCursos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal duracion { get; set; }
        public string objetivo { get; set; }
        public string temasPrincipales { get; set; }
        public string claveCurso { get; set; }
        public string referenciasNormativas { get; set; }
        public string nota { get; set; }
        public EstatusCursoEnum estatus { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
        public TematicaCursoEnum tematica { get; set; }
        public bool esGeneral { get; set; }
        public bool aplicaTodosPuestos { get; set; }
        public bool capacitacionUnica { get; set; }
        public bool isActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        public DateTime fechaEdicion { get; set; }
        public int usuarioEdicion { get; set; }
        public virtual List<tblS_CapacitacionCursosExamen> Examenes { get; set; }
        public virtual List<tblS_CapacitacionCursosPuestos> Puestos { get; set; }
        public virtual List<tblS_CapacitacionCursosPuestosAutorizacion> PuestosAutorizacion { get; set; }
        public virtual List<tblS_CapacitacionCursosMando> Mandos { get; set; }
        public virtual List<tblS_CapacitacionCursosCC> CentrosCosto { get; set; }
        public int division { get; set; }
        public bool reglaPersonalAutorizado { get; set; }
    }
}
