using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_SUA_Resumen
    {
        public int id { get; set; }
        public int suaID { get; set; }
        public string cc { get; set; }
        public string ac { get; set; }
        public string estado { get; set; }
        public string registroPatronal { get; set; }
        public string descripcionCC { get; set; }
        public decimal imssPatronal { get; set; }
        public decimal imssObrero { get; set; }
        public decimal rcvPatronal { get; set; }
        public decimal rcvObrero { get; set; }
        public decimal infonavit { get; set; }
        public decimal amortizacion { get; set; }
        public decimal isn { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("suaID")]
        public virtual tblC_Nom_SUA sua { get; set; }
    }
}

