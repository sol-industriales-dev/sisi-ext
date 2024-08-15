using Core.Entity.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class BarrenacionCostoDTO
    {
        public int id { get; set; }
        public decimal manoObra { get; set; }
        public decimal costoRenta { get; set; }
        public decimal diesel { get; set; }
        public decimal totalCosto { get; set; }
        public bool activa { get; set; }
        public DateTime fechaCosto { get; set; }
        public decimal? totalPieza { get; set; }   
        public decimal? totalOtro { get; set; }
    }
}
