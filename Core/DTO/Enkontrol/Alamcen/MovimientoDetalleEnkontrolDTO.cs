using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class MovimientoDetalleEnkontrolDTO
    {
        public int almacen { get; set; }
        public string almacenDesc { get; set; }
        public int tipo_mov { get; set; }
        public int? numero { get; set; }
        public int? remision { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public string descInsumo { get; set; }
        public string comentarios { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public string areaCuenta { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public int? partida_oc { get; set; }
        public decimal? costo_prom { get; set; }
        public int? sector_id { get; set; }

        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }

        public string unidad { get; set; }
        public decimal? minimo { get; set; }
        public decimal? existencia { get; set; }

        public decimal cantidadPendiente { get; set; }
        public decimal cant_recibida { get; set; }
        public int moneda { get; set; }
        public string monedaDesc { get; set; }
        public string minimoDesc { get; set; }
        public decimal solicitadoPendiente { get; set; }
        public int partidaRequisicion { get; set; }
        public int numeroRequisicion { get; set; }

        public string PERU_insumo { get; set; }
        public string noEconomico { get; set; }
    }
}
