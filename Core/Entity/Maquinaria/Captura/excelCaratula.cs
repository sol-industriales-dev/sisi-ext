using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class excelCaratula
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public string grupo { get; set; }
        public string modelo { get; set; }
        public int unidad { get; set; }
        public decimal costo { get; set; }
        public decimal cargoFijo { get; set; }
        public decimal overhaul { get; set; }
        public decimal mtoCorrectivo { get; set; }
        public decimal combustible { get; set; }
        public decimal aceites { get; set; }
        public decimal filtros { get; set; }
        public decimal ansul { get; set; }
        public decimal llantas { get; set; }
        public decimal carrileria { get; set; }
        public decimal desgasteHerramientas { get; set; }
        public decimal cargoOperador { get; set; }
        public decimal personalMto { get; set; }
        public int moneda { get; set; }
        public bool manoObra { get; set; }
    }
}
