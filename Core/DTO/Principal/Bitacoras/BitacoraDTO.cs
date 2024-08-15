using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Bitacoras
{
    public class BitacoraDTO
    {
        public long id { get; set; }
        public string modulo { get; set; }
        public string accion { get; set; }
        public string usuario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public long registroID { get; set; }
        public string objeto { get; set; }
        public string publicIP { get; set; }
        public string localIP { get; set; }
    }
}
