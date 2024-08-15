using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Estrellas
    {
        public int id { get; set; }
        public int estrellas { get; set; }
        public string descripcion { get; set; }
        public decimal minimo { get; set; }
        public decimal maximo { get; set; }
        public bool estatus { get; set; }
    }
}
