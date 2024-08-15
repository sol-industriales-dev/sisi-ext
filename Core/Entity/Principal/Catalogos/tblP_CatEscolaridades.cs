using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Catalogos
{
    public class tblP_CatEscolaridades
    {
        public int id { get; set; }
        public string escolaridad { get; set; }
        public int orden { get; set; }
        public bool registroActivo { get; set; }
    }
}
