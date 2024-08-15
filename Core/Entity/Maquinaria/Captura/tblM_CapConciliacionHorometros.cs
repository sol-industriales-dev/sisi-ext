using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapConciliacionHorometros
    {
        public int id { get; set; }
        public int idCapCaratula { get; set; }
        public int idEncCaratula { get; set; }
        public int numero { get; set; }
        public int noEconomicoID { get; set; }
        public string economico { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public decimal horometroInicial { get; set; }
        public decimal horometroFinal { get; set; }
        public decimal horometroEfectivo { get; set; }
        public int unidad { get; set; }
        public decimal costo { get; set; }
        public decimal total { get; set; }
        public int idEmpresa { get; set; }
        public string observaciones { get; set; }

        public int moneda { get; set; }

        public decimal overhaul { get; set; }
    }
}
