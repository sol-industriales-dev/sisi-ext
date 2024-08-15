using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.ControlPresupuestal
{
    public class MovimientoPolizaDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public DateTime fechapol { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public decimal monto { get; set; }
        public int area { get; set; }
        public int cuenta_oc { get; set; }
        public string economico { get; set; }
        public string cuentaDescripcion { get; set; }
        public string concepto { get; set; }
        public int conceptoID { get; set; }
    }
}
