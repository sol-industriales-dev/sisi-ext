using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoAcuerdos
{
    public class tblSA_PromoverActividad
    {
        public int id { get; set; }
        public int actividadID { get; set; }
        public string observacion { get; set; }
        public int columna { get; set; }
        public bool estatus { get; set; }
        public int jefeID { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaAccion { get; set; }
        public int accion { get; set; }
        public virtual tblSA_Actividades actividad { get; set; }
        public int responsableID { get; set; }
    }
}
