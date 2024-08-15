using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_Pago
    {
        public int Id { get; set; } 
        public int PeriodoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string ArchivoPago { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModificacionId { get; set; }
        public decimal PagoParcial { get; set; }
        public int ContratoID { get; set; }

      //  [ForeignKey("PeriodoId")]
     //   public virtual tblAF_DxP_ContratoDetalle ContratoDetalle { get; set; }
        [ForeignKey("UsuarioCreacionId")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioModificacionId")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }
    }
}