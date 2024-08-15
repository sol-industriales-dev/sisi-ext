using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class PlanMaestroDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int periodo { get; set; }
        public string periodoDesc { get; set; }
        public int areaID { get; set; }
        public string area { get; set; }
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }
        public bool checkPlanMes { get; set; }
        public bool mes1 { get; set; }
        public bool mes2 { get; set; }
        public bool mes3 { get; set; }
        public bool mes4 { get; set; }
        public bool mes5 { get; set; }
        public bool mes6 { get; set; }
        public bool mes7 { get; set; }
        public bool mes8 { get; set; }
        public bool mes9 { get; set; }
        public bool mes10 { get; set; }
        public bool mes11 { get; set; }
        public bool mes12 { get; set; }
        public List<int> mesesFiltro { get; set; }
        public List<int> periodoFiltro { get; set; }
    }
}
