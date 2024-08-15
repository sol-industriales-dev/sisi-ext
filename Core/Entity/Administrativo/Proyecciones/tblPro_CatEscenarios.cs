using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_CatEscenarios
    {

        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal PFinal { get; set; }
        public decimal Pinicial { get; set; }
        public bool estatus { get; set; }
        public int PadreID { get; set; }
        public int ordenID { get; set; }
        public int nivel { get; set; }
    }
}
