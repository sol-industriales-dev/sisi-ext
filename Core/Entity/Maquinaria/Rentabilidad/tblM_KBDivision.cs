using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBDivision
    {
        public int id { get; set; }
        public string division { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }

        public virtual List<tblM_KBDivisionDetalle> divisionDetalle { get; set; }

        
    }
}
