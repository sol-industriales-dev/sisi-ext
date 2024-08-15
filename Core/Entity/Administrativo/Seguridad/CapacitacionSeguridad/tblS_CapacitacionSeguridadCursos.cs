using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadCursos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal duracion { get; set; }
        public string objetivo { get; set; }
        public string temasPrincipales { get; set; }
        public string claveCurso { get; set; }
        public string referenciasNormativas { get; set; }
        public string nota { get; set; }
        public int estatus { get; set; }
        public int clasificacion { get; set; }
        public int tematica { get; set; }
        public bool esGeneral { get; set; }
        public bool aplicaTodosPuestos { get; set; }
        public bool capacitacionUnica { get; set; }
        public bool isActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        public DateTime fechaEdicion { get; set; }
        public int usuarioEdicion { get; set; }
        public virtual List<tblS_CapacitacionSeguridadCursosExamen> Examenes { get; set; }
        public virtual List<tblS_CapacitacionSeguridadCursosPuestos> Puestos { get; set; }
        public virtual List<tblS_CapacitacionSeguridadCursosPuestosAutorizacion> PuestosAutorizacion { get; set; }
        public virtual List<tblS_CapacitacionSeguridadCursosMando> Mandos { get; set; }
        public virtual List<tblS_CapacitacionSeguridadCursosCC> CentrosCosto { get; set; }
        public int division { get; set; }
        public bool reglaPersonalAutorizado { get; set; }
    }
}
