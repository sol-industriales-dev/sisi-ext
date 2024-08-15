using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CatAceitesLubricantes
    {
        public int id { get; set; }
        public string Descripcion { get; set; }
        public int modeloID { get; set; }
        public int subConjuntoID { get; set; }
        public bool registroActivo { get; set; }
    }
}
