using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class DetallesKubrixPorCtaDTO
    {
        public string cta { get; set; }
        public string scta { get; set; }
        public string sscta { get; set; }
        public string descripcion { get; set; }
        public string anio { get; set; }
        public string mes { get; set; }
        public string poliza { get; set; }
        public string tp { get; set; }
        public string fecha { get; set; }
        public string cc { get; set; }
        public string referencia { get; set; }
        public string concepto { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public decimal saldo { get; set; }
    }
}
