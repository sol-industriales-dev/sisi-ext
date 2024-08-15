using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria
{
    public class tblM_CorreosEnvioInventario
    {
        public int id { get; set; }
        public string correo { get; set; }
        public bool estatus { get; set; }
        public int TipoEnvio { get; set; }
    }
}
