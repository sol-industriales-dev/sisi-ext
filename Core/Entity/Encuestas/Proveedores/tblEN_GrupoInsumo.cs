using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas.Proveedores
{
    public class tblEN_GrupoInsumo
    {
        public int Id { get; set; }
        public int TipoEncuestaId { get; set; }
        public int TipoInsumo { get; set; }
        public int GrupoInsumo { get; set; }
        public string Descripcion { get; set; }
        public bool Familia { get; set; }
        public int? Consecutivo { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("TipoEncuestaId")]
        public virtual tblEN_TipoEncuestaProveedor TipoEncuesta { get; set; }
    }
}