using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBDivisionDetalle
    {

        public int id { get; set; }
        public int acID { get; set; }
        public int divisionID { get; set; }
        public bool estatus { get; set; }
        public virtual tblM_KBDivision division { get; set; }

        public virtual tblP_CC ac { get; set; }

    }
}
