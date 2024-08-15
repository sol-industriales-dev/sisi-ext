using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_Divisiones
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string abreviacion { get; set; }
        public bool esActivo { get; set; }
    }
}
