using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_ProveedoresSocios
    {
        public int id { get; set; }
        public int FK_idProv { get; set; }        
        public decimal FK_numpro { get; set; }
        public string socios { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
