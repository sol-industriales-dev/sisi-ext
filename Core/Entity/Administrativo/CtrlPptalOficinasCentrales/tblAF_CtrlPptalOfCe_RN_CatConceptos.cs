using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_RN_CatConceptos
    {
        public int id { get; set; }
        public int idRNAgrupacion { get; set; }
        public string concepto { get; set; }
        public decimal cantidad { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
