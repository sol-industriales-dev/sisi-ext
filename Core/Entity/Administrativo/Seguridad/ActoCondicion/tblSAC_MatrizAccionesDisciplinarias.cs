using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.ActoCondicion
{
    public class tblSAC_MatrizAccionesDisciplinarias
    {
        public int id { get; set; }
        public int numero { get; set; }
        public string tipoInfraccion { get; set; }
        public int nivel { get; set; }
        public int amonestacion { get; set; }
        public int suspension { get; set; }
        public int rescision { get; set; }
        public int sancion { get; set; }
        public bool estatus { get; set; }
    }
}
