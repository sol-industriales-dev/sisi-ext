using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class DepreciacionMaquinaConOverhaulDTO
    {
        public int IdMaquina { get; set; }
        public string NoEconomico { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal DepreciacionEquipo { get; set; }
        public decimal DepreciacionEquipoSemanal { get; set; }
        public decimal DepreciacionEquipoPeriodo { get; set; }
        public decimal DepreciacionOverhaul { get; set; }
        public decimal DepreciacionOverhaulSemanal { get; set; }
        public decimal DepreciacionOverhaulPeriodo { get; set; }
        public decimal MoiEquipo { get; set; }
        public decimal MoiOverhaul { get; set; }
        public decimal DepreciacionMensualEquipo { get; set; }
        public decimal DepreciacionMensualOH { get; set; }
    }
}