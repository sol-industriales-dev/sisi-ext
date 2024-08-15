using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Catalogos
{
    public class tblP_EstadoCivil
    {
        public int id { get; set; }
        public string estadoCivil { get; set; }
        public bool registroActivo { get; set; }
    }
}
