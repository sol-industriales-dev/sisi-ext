using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo
{
    public class CalificacionPartidaDTO
    {
        public int proveedor { get; set; }
        public int partida { get; set; }
        public decimal calificacion { get; set; }
    }
}
