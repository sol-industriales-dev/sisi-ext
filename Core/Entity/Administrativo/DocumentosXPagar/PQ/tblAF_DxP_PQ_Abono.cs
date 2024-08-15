using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_Abono
    {
        public int id { get; set; }
        public int pqID { get; set; }
        public decimal importe { get; set; }
        public DateTime fecha { get; set; }
        public string poliza { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreacionID { get; set; }

        [ForeignKey("pqID")]
        public virtual tblAF_DxP_Contrato pq { get; set; }

        [ForeignKey("usuarioCreacionID")]
        public virtual tblP_Usuario usuario { get; set; }
    }
}
