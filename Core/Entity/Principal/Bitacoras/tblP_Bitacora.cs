using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Bitacoras
{
    public class tblP_Bitacora
    {
        public long id { get; set; }
        public long modulo { get; set; }
        public int accion { get; set; }
        public long usuarioID { get; set; }
        public DateTime fecha { get; set; }
        public long registroID { get; set; }
        public string objeto { get; set; }
        public string publicIP { get; set; }
        public string localIP { get; set; }
    }
}
