using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class DetalleDTO
    {
        public string descripcion { get; set; }
        public string importe { get; set; }
        public List<DetalleDTO> detalles { get; set; }
    }
}
