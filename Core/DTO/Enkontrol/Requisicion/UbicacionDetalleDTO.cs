using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class UbicacionDetalleDTO
    {
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public string almacenLAB { get; set; }

        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public decimal cantidad { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }

        public decimal cantidadMovimiento { get; set; }
        public string ultimoConsumoString { get; set; }
        public string ultimaCompraString { get; set; }

        public int? area { get; set; }
        public int? cuenta { get; set; }
        public int almacen { get; set; }
    }
}
