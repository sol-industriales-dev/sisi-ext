using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Evaluacion
{
    public class tblSED_Evaluacion
    {
        public int id { get; set; }
        public int empleadoID { get; set; }
        public int actividadID { get; set; }
        public string rutaEvidencia { get; set; }
        public string comentariosEmpleado { get; set; }
        public DateTime fechaActividad { get; set; }
        public DateTime fechaCaptura { get; set; }
        public decimal ponderacionActual { get; set; }
        public PeriodicidadEnum periodicidadActual { get; set; }
        public bool aplica { get; set; }
        public int evaluadorID { get; set; }
        public string comentariosEvaluador { get; set; }
        public bool aprobado { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
