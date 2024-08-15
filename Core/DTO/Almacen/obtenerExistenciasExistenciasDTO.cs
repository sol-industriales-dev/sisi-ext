using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class obtenerExistenciasExistenciasDTO
    {
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public decimal existencia { get; set; }

    }
}
