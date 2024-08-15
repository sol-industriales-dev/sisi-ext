using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapCombustible
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public string CC { get; set; }
        public DateTime fecha { get; set; }
        public int turno { get; set; }
        public decimal volumne_carga { get; set; }
        public string surtidor { get; set; }
        public decimal Carga1 { get; set; }
        public decimal HorometroCarga1 { get; set; }
        public decimal Carga2 { get; set; }
        public decimal HorometroCarga2 { get; set; }
        public decimal Carga3 { get; set; }
        public decimal HorometroCarga3 { get; set; }
        public decimal Carga4 { get; set; }
        public decimal HorometroCarga4 { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PrecioLitro { get; set; }
        public bool aplicarCosto { get; set; }
        public string pipa1 { get; set; }
        public string pipa2 { get; set; }
        public string pipa3 { get; set; }
        public string pipa4 { get; set; }
        public DateTime FechaCaptura { get; set; }
    }
}
