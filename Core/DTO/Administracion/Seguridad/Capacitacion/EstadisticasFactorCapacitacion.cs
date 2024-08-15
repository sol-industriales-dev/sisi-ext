using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class EstadisticasFactorCapacitacion
    {
        public string proyecto { get; set; }
        //public decimal porcentaje { get; set; }
        public decimal porcentajeCapacitacion { get; set; }
        public decimal porcentajeEfectividadCiclos { get; set; }
        public decimal porcentajeRecorridos { get; set; }
        public decimal porcentajeCincoS { get; set; }
        public decimal factorCapacitacionCentroCosto { get; set; }
    }
}
