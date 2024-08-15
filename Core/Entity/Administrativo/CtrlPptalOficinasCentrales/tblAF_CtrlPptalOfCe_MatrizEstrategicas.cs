using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPresupuestalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_MatrizEstrategicas
    {
        public int id { get; set; }
        public int consecutivo { get; set; }
        public string cuenta { get; set; }
        public string actividades { get; set; }
        public decimal cantidad { get; set; }
        public int idResponsable { get; set; }
        public DateTime? fechaReal { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
