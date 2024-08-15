using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNCicloTrabajoRegistro
    {
        public int id { get; set; }
        public string cc { get; set; }
        public DateTime? fecha { get; set; }
        //public int area { get; set; }
        public int empresa { get; set; }
        public int cicloID { get; set; }
        public int revisor { get; set; }
        public int colaborador { get; set; }
        public decimal calificacion { get; set; }
        public string economico { get; set; }
        public bool acredito { get; set; }
        public bool retroalimentacion { get; set; }
        public string accionRequerida { get; set; }
        public int metodo { get; set; }
        public int cursoID { get; set; }
        public string rutaEvidencia { get; set; }
        public int evaluador { get; set; }
        public bool aprobo { get; set; }
        public string comentariosEvaluador { get; set; }
        public string observacionesRevisor { get; set; }
        public string accionesTomadas { get; set; }
        public string observacionesLider { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
