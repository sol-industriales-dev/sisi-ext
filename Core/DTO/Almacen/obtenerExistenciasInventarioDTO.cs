using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class obtenerExistenciasInventarioDTO
    {
        public string centro_costo { get; set; }
        public int almacen { get; set; }
        public string fecha { get; set; }
        public int insumo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public string importe { get; set; }
        public string partida { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public string origen { get; set; }
        public string tipo_mov { get; set; }
        public string insumoDesc { get; set; }

    }
}
