using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_DivisionCC
    {
        public int id { get; set; }
        public string cc { get; set; }

        public int division_id { get; set; }
        public virtual tblCO_Divisiones division { get; set; }
    }
}
