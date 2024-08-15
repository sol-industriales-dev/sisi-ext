using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPresupuestalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_CatConceptos
    {
        public int id { get; set; }
        public int idAgrupacion { get; set; }
        public int idConcepto { get; set; }
        public string concepto { get; set; }
        public int insumo { get; set; }
        public string insumoDescripcion { get; set; }
        //public int cta { get; set; }
        //public int scta { get; set; }
        //public int sscta { get; set; }
        public string cuentaDescripcion { get; set; }
        public decimal cantPpto { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
