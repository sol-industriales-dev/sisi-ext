using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoAcuerdos
{
    public class tblSA_Actividades
    {
        public int id { get; set; }
        public int minutaID { get; set; }
        public int columna { get; set; }
        public int orden { get; set; }
        public string tipo { get; set; }
        public string actividad { get; set; }
        public string descripcion { get; set; }
        public int responsableID { get; set; }
        public string responsable { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaCompromiso { get; set; }
        public int prioridad { get; set; }
        public int comentariosCount { get; set; }
        public virtual tblSA_Minuta minuta { get; set; }
        public virtual List<tblSA_Comentarios> comentarios { get; set; }
        public bool enVersion { get; set; }
        public int revisaID { get; set; }
        public string revisa { get; set; }
    }
}
