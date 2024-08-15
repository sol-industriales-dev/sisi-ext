using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_AutorizarProveedor
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public bool PrimerVobo { get; set; }
        public bool PuedeVobo { get; set; }
        public bool PuedeAutorizar { get; set; }
        public bool registroActivo { get; set; }

    }
}
