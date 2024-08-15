using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_ADP_Facultamientos
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public string cc { get; set; }
        public string usuarioCreacion { get; set; }
        public DateTime? fechaCrecion { get; set; }
        public string usuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
        public int idUsuario { get; set; }
    }
}
