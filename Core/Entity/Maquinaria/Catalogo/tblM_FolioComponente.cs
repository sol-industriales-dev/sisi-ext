using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_FolioComponente
    {
        public int id { get; set; }
        public int modeloID { get; set; }
        public int conjuntoID { get; set; }
        public int subConjuntoID { get; set; }
        public int cc { get; set; }
      //  public int posicionID { get; set; }
        public int Folio { get; set; }
        public string prefijo { get; set; }
    }
}
