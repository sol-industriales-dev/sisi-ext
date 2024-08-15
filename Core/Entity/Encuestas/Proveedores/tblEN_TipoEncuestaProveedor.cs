using Core.Enum.Encuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas.Proveedores
{
    public class tblEN_TipoEncuestaProveedor
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public tipoDependenciaProveedor tipoDependencia { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }

        [ForeignKey("TipoEncuestaId")]
        public virtual List<tblEN_UsuarioInsumo> UsuarioInsumo { get; set; }
    }
}
