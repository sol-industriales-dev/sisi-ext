using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class sc_movpolDTO
    {
        public int Year { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public int Linea { get; set; }
        public int Cta { get; set; }
        public int Scta { get; set; }
        public int Sscta { get; set; }
        public int Digito { get; set; }
        public int TM { get; set; }
        public string Referencia { get; set; }
        public string Cc { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public int ITM { get; set; }
        public DateTime FechaPol { get; set; }
        public DateTime? FechaFactura { get; set; }
        public DateTime? FechaCFD { get; set; }
        public bool Match { get; set; }
        public string PolizaAlta { get; set; }
        public int? Area { get; set; }
        public int? Cuenta_OC { get; set; }
        public string AreaCuentaDescripcion { get; set; }
        public string AreaCuenta { get; set; }
        public string Factura { get; set; }
        public string NumEconomico { get; set; }

        public string ccDescripcion { get; set; }

        public bool bajaCosto { get; set; }
        public int idBajaCosto { get; set; }
        public int semanas { get; set; }
    }
}