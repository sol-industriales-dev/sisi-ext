using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Evento_Det
    {
        public int id { get; set; }
        public int eventoID { get; set; }
        public virtual tblRH_BN_Evento evento { get; set; }
        public int cveEmp { get; set; }
        public string nombreEmp { get; set; }
        public string puestoEmp { get; set; }
        public int tipoNomina { get; set; }
        public decimal monto { get; set; }
        public int estatus { get; set; }
        public DateTime fechaAplicacion { get; set; }
        public int periodoNomina { get; set; }
    }
}
