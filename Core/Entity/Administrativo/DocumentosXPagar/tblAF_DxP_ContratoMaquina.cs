
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_ContratoMaquina
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int MaquinaId { get; set; }
        public decimal Credito { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModificacionId { get; set; }
        public decimal porcentaje { get; set; }


        [ForeignKey("ContratoId")]
        public virtual tblAF_DxP_Contrato Contrato { get; set; }
        [ForeignKey("MaquinaId")]
        public virtual tblM_CatMaquina Maquina { get; set; }
        [ForeignKey("UsuarioCreacionId")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioModificacionId")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }
    }
}