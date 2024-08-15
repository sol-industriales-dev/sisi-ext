using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Enkontrol.Requisicion;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class SurtidoCompraDTO
    {
        public int partida { get; set; }
        public int insumo { get; set; }
        public decimal cantidad { get; set; }
        public decimal surtido { get; set; }
        public decimal aSurtir { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }

        public List<UbicacionDetalleDTO> listUbicacionMovimiento { get; set; }
    }
}
