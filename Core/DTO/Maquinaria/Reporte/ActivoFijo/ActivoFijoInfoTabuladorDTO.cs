using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoInfoTabuladorDTO
    {
        public decimal Monto { get; set; }
        public decimal DepreciacionTotal { get; set; }
        public int MesesTotalesDepreciacion { get; set; }
        public int MesesFaltantesDepreciacion { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public List<ActivoFijoTabuladorDTO> Tabulador { get; set; }
    }
}