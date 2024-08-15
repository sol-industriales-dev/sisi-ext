using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CuentasPorCobrar
{
    public class tblCXC_DivisionDetalle
    {
        public int id { get; set; }
        public int acID { get; set; }
        public int divisionID { get; set; }
        public bool estatus { get; set; }
        public virtual tblP_CC ac { get; set; }
    }
}
