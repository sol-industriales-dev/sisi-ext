using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class PlanMes_DetalleDTO
    {
        public int id { get; set; }
        public int planMesID { get; set; }
        public int tipo { get; set; }
        public string cuadrilla { get; set; }
        public string periodo { get; set; }
        public int equipoID { get; set; }
        public string equipo { get; set; }
        public List<int> dias { get; set; }

        public bool checkRev { get; set; }
        public int revisionID { get; set; }

        public string equipoAreaDesc { get; set; }
    }
}
