using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoRegistrarCatDepDTO
    {
        public int Id { get; set; }
        public int IdCatMaquina { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public int MesesTotalesADepreciar { get; set; }
        public DateTime FechaInicioDepreciacion { get; set; }
        public DateTime? FechaInicioDepreciacionFiscal { get; set; }
        public List<ActivoFijoRegistroPolDTO> Polizas { get; set; }
    }
}