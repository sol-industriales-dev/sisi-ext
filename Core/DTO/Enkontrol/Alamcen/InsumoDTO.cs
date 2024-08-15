using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class InsumoDTO
    {
        public string id { get; set; }
        public string value { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public decimal exceso { get; set; }
        public bool isAreaCueta { get; set; }
        public string cancelado { get; set; }
        public decimal costoPromedio { get; set; }
        public decimal costoPromedioEntrada { get; set; }
        public decimal costo { get; set; }
        public int color_resguardo { get; set; }
        public int compras_req { get; set; }

        public int insumo { get; set; }
        public decimal precio { get; set; }
        public int numValue { get; set; }
    }
}
