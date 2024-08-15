using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlAditiva
    {
        public int id { get; set; }
        public int capPptosId { get; set; }
        public bool autorizado { get; set; }
        public bool rechazado { get; set; }
        public int? autorizante1 { get; set; }
        public int? autorizante2 { get; set; }
        public int? autorizante3 { get; set; }
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
        public string comentario { get; set; }
        public string comentarioRechazo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("capPptosId")]
        public virtual tblAF_CtrlPptalOfCe_CapPptos presupuesto { get; set; }

        [ForeignKey("autorizante1")]
        public virtual tblP_Usuario autorizanteUno { get; set; }
        
        [ForeignKey("autorizante2")]
        public virtual tblP_Usuario autorizanteDos { get; set; }
        
        [ForeignKey("autorizante3")]
        public virtual tblP_Usuario autorizanteTres { get; set; }

        [ForeignKey("idUsuarioCreacion")]
        public virtual tblP_Usuario usuarioCreacion { get; set; }
        
        [ForeignKey("idUsuarioModificacion")]
        public virtual tblP_Usuario usuarioModificacion { get; set; }
    }
}
