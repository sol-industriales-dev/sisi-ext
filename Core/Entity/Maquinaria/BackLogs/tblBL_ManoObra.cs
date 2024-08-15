using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_ManoObra
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int idBackLog { get; set; }
        public bool esActivo { get; set; }
    }
}
