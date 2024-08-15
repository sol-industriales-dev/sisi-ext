using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class PlanMesDTO
    {
        public int id { get; set; }
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }
        public int periodo { get; set; }
        public string periodoDesc { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public List<PlanMes_DetalleDTO> detalle { get; set; }
    }
}
