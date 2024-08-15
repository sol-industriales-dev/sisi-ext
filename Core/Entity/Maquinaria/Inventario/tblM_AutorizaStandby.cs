using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
   public class tblM_AutorizaStandby
    {
        public int id { get; set; }
        public int idElabora { get; set; }
        public int idGerente { get; set; }
        public DateTime FechaElaboro { get; set; }
        public DateTime FechaValida { get; set; }
        public string CadenaElabora { get; set; }
        public string CadenaGerente { get; set; }
        public bool FirmaElabora { get; set; }
        public bool FirmaGerente { get; set; }

        public int standByID { get; set; }
    }
}
