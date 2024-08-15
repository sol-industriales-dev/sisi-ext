using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public class CatEscenariosDTO
    {
        public int id { get; set; }
        public string Padre { get; set; }
        public string Hijo { get; set; }
        public bool estatus { get; set; }
    }
}
