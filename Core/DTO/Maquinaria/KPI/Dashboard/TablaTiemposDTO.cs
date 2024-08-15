using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class TablaTiemposDTO
    {
        public decimal hrsProgramado { get; set; }
        public decimal hrsDisponible { get; set; }
        public decimal porDisponible { get; set; }
        public decimal hrsMantenimiento { get; set; }
        public decimal porMantenimiento { get; set; }
        public decimal hrsOperacion { get; set; }
        public decimal porOperacion { get; set; }
        public decimal hrsTrabajo { get; set; }
        public decimal porTrabajo { get; set; }
        public decimal hrsDemora { get; set; }
        public decimal porDemora { get; set; }
        public decimal hrsParado { get; set; }
        public decimal porParado { get; set; }
        public decimal hrsProgramadoSM { get; set; }
        public decimal porProgramadoSM { get; set; }
        public decimal hrsNoProgramadoUM { get; set; }
        public decimal porNoProgramadoUM { get; set; }

        public List<InfoParosDTO> paros { get; set; }
    }
}
