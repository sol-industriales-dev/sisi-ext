using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class KubrixCorteDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public string tp { get; set; }
        public int poliza { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta_oc { get; set; }
        public DateTime fechapol { get; set; }
        public string referencia { get; set; }
    }
}
