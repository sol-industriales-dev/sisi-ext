using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas.Proveedores
{
    public class tblEN_Top20Proveedores
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public int TipoTop20Id { get; set; }
        public int TipoEncuestaId { get; set; }
        public int CantidadEvaluaciones { get; set; }
        public DateTime FechaTop20 { get; set; }
        public int UsuarioId { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("TipoTop20Id")]
        public virtual tblEN_TipoTop20 TipoTop20 { get; set; }

        [ForeignKey("TipoEncuestaId")]
        public virtual tblEN_TipoEncuestaProveedor TipoEncuesta { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual tblP_Usuario Usuario { get; set; }
    }
}