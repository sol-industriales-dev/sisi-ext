using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo
{
    [NotMapped]
    public class CalificacionConfiabilidadDTO : tblCom_CC_Calificacion
    {
        public decimal ValorPrecio { get; set; }
        public DateTime ValorTiempoEntrega { get; set; }
        public decimal ValorCondicionPago { get; set; }
        public decimal ValorLAB { get; set; }
        public decimal ValorConfiabilidad { get; set; }
        public decimal ValorCalidad { get; set; }
        public decimal ValorServicioPostVenta { get; set; }

        public List<CalificacionConfiabilidadPartidaDTO> partidas { get; set; }
    }

    [NotMapped]
    public class CalificacionConfiabilidadPartidaDTO : tblCom_CC_CalificacionPartida
    {
        public int partida { get; set; }
        public decimal ValorPrecio { get; set; }
    }
}
