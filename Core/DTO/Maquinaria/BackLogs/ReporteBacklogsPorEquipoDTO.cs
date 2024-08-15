using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Maquinaria.BackLogs;
using Core.Entity.Maquinaria.BackLogs;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class ReporteBacklogsPorEquipoDTO
    {
        public List<int> lstInsumos { get; set; }
        public string noEconomico { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public decimal horas { get; set; }
        public string estatus { get; set; }
        public DateTime? fechaUltimoBL { get; set; }
        public int cantidadBL { get; set; }
        public decimal presupuestoMes { get; set; }
        public decimal presupuestoAcumulado { get; set; }
        public decimal costoHora { get; set; }
        public List<BackLogsDTO> backlogs { get; set; }
        public string strPresupuestoMes { get; set; }
        public string strPresupuestoAcumulado { get; set; }
    }
}
