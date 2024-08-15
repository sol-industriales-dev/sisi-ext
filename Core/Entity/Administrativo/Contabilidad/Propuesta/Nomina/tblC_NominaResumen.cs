using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina
{
    public class tblC_NominaResumen
    {
        public int id { get; set; }
        public int tipoNomina { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public int tipoCuenta { get; set; }
        public string cc { get; set; }
        public decimal nomina { get; set; }
        public decimal iva { get; set; }
        public decimal retencion { get; set; }
        public decimal total { get; set; }
        public decimal noEmpleado { get; set; }
        public int noPracticante { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}
