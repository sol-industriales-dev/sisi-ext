using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Facturacion.Estimacion
{
    public class tblF_EstimacionResumen
    {
        public int id { get; set; }
        public string numcte { get; set; }
        public string cc { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechavenc { get; set; }
        public string linea { get; set; }
        public decimal estimacion { get; set; }
        public decimal anticipo { get; set; }
        public decimal vencido { get; set; }
        public decimal pronostico { get; set; }
        public decimal cobrado { get; set; }
        public DateTime fechaResumen { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool Enkontrol { get; set; }
    }
}
