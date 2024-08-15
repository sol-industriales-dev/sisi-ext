using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_SUA_Det
    {
        public int id { get; set; }
        public int suaID { get; set; }
        public int clave_empleado { get; set; }
        public string nss { get; set; }
        public string rfc { get; set; }
        public string nombre { get; set; }
        public string cc { get; set; }
        public int dias { get; set; }
        public decimal salarioDiario { get; set; }
        public int lic { get; set; }
        public int incapacidades { get; set; }
        public int ausentismos { get; set; }
        public decimal cuotaFija { get; set; }
        public decimal excedentePatronal { get; set; }
        public decimal excedenteObrera { get; set; }
        public decimal prestacionesPatronal { get; set; }
        public decimal prestacionesObrera { get; set; }
        public decimal gastosMedicosPatronal { get; set; }
        public decimal gastosMedicosObrera { get; set; }
        public decimal riesgosTrabajo { get; set; }
        public decimal invalidezVidaPatronal { get; set; }
        public decimal invalidezVidaObrera { get; set; }
        public decimal guarderiasPrestaciones { get; set; }
        public decimal patronal { get; set; }
        public decimal obrera { get; set; }
        public decimal subtotal { get; set; }
        public decimal rcvRetiro { get; set; }
        public decimal rcvPatronal { get; set; }
        public decimal rcvObrera { get; set; }
        public decimal infonavitAportacionPatronal { get; set; }
        public decimal infonavitAmortizacion { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("suaID")]
        public virtual tblC_Nom_SUA sua { get; set; }
    }
}
