using Core.Entity.Administrativo.CtrlPresupuestalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_CapPptos
    {
        public int id { get; set; }
        public string actividad { get; set; }
        public string cc { get; set; }
        public int idAgrupacion { get; set; }
        public int idConcepto { get; set; }
        public decimal importeEnero { get; set; }
        public decimal importeFebrero { get; set; }
        public decimal importeMarzo { get; set; }
        public decimal importeAbril { get; set; }
        public decimal importeMayo { get; set; }
        public decimal importeJunio { get; set; }
        public decimal importeJulio { get; set; }
        public decimal importeAgosto { get; set; }
        public decimal importeSeptiembre { get; set; }
        public decimal importeOctubre { get; set; }
        public decimal importeNoviembre { get; set; }
        public decimal importeDiciembre { get; set; }
        public decimal importeTotal { get; set; }
        public int anio { get; set; }
        public int idResponsable { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("capPptosId")]
        public virtual List<tblAF_CtrlAditiva> aditivas { get; set; }

        [ForeignKey("idConcepto")]
        public virtual tblAF_CtrlPptalOfCe_CatConceptos concepto { get; set; }

        [ForeignKey("idAgrupacion")]
        public virtual tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion { get; set; }
    }
}
