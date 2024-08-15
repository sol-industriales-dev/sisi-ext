using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Enkontrol.Compras.Requisicion;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class RequisicionDetDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public string insumoDescripcion { get; set; }
        public DateTime fecha_requerido { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal cant_ordenada { get; set; }
        public DateTime? fecha_ordenada { get; set; }
        public string estatus { get; set; }
        public decimal cant_cancelada { get; set; }
        public string referencia_1 { get; set; }
        public decimal cantidad_excedida_ppto { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string observaciones { get; set; }
        public string noEconomico { get; set; }

        public decimal cantidadConfirmada { get; set; }
        public int idReq { get; set; }
        public string insumoDesc { get; set; }
        public string unidad { get; set; }
        public string cancelado { get; set; }
        public int? compras_req { get; set; }
        public string partidaDesc { get; set; }
        public decimal cantidadCapturada { get; set; }

        public List<SurtidoDetDTO> listaSurtido { get; set; }
        public decimal totalASurtir { get; set; }
        public decimal existenciaTotal { get; set; }
        public decimal existenciaLAB { get; set; }
        public bool validadoCompras { get; set; }
        public bool validadoAlmacen { get; set; }
    }
}
