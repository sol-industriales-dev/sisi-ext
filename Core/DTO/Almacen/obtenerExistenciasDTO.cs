using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class obtenerExistenciasDTO
    {
        public string insumo { get; set; }
        public string ubicacion { get; set; }
        public string categoria { get; set; }
        public decimal fisica { get; set; }
        public decimal teorica { get; set; }
        public decimal diferencia { get; set; }
        public decimal promedioOPrecio { get; set; }
        public string cargos { get; set; }
        public string abono { get; set; }
    }
}
