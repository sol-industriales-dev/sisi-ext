using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas.Proveedores
{
    public class tblEN_UsuariosAC
    {
        public int Id { get; set; }
        public int UsuarioInsumoId { get; set; }
        public string AreaCuenta { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("UsuarioInsumoId")]
        public virtual tblEN_UsuarioInsumo UsuarioInsumo { get; set; }
    }
}