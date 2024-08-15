using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class CuentasPendientesDTO
    {
        public string responsable { get; set; }
        public string numcte { get; set; }
        public string factura { get; set; }
        public decimal monto { get; set; }
        public string concepto { get; set; }
        public string areaCuenta { get; set; }
        public string areaCuentaDesc { get; set; }
        public decimal montoPronosticado { get; set; }
        public DateTime fecha { get; set; }
        public bool esCorte { get; set; }
        public int? idAcuerdo { get; set; }
        public bool esRemoved { get; set; }
        public string comentarios { get; set; }
        public decimal montoPagado { get; set; }
        public int idDivision { get; set; }
        public string descDivision { get; set; }
        public DateTime fechaOrig { get; set; }
        public DateTime fechaCorte { get; set; }
        public DateTime fechaOGVenc { get; set; }
    }
}
