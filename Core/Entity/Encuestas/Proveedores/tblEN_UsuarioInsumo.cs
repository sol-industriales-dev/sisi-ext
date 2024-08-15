using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas.Proveedores
{
    public class tblEN_UsuarioInsumo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EmpleadoId { get; set; }
        public int TipoEncuestaId { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("UsuarioId")]
        public tblP_Usuario Usuario { get; set; }

        [ForeignKey("TipoEncuestaId")]
        public tblEN_TipoEncuestaProveedor TipoEncuesta { get; set; }
    }
}