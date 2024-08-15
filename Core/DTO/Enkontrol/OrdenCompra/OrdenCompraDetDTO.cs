using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class OrdenCompraDetDTO
    {
        #region key
        public string cc { get; set; }
        public int numero { get; set; }
        #endregion
        #region Requisicion
        public int num_requisicion { get; set; }
        public int part_requisicion { get; set; }
        public decimal cantidadRequisicion { get; set; }
        public int insumo { get; set; }
        public DateTime? fecha_entrega { get; set; }
        public string fecha_entregaString { get; set; }
        public decimal cantidad { get; set; }
        public DateTime? fecha_recibido { get; set; }
        public decimal cant_canc { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public decimal cant_ordenada { get; set; } 
        #endregion
        public int partida { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public decimal ajuste_cant { get; set; }
        public decimal ajuste_imp { get; set; }       
        public decimal cant_recibida { get; set; }
        public decimal imp_recibido { get; set; }        
        public decimal imp_canc { get; set; }
        public decimal? acum_ant { get; set; }
        public decimal? max_orig { get; set; }
        public decimal? max_ppto { get; set; }
        public decimal? porcent_iva { get; set; }
        public decimal? iva { get; set; }
        public bool exento_iva { get; set; }
        public string descripcion { get; set; }

        public string insumoDesc { get; set; }
        public int moneda { get; set; }
        public string monedaDesc { get; set; }
        public string areaCuenta { get; set; }
        public string areaCuentaDesc { get; set; }
        public string partidaDescripcion { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }

        public decimal surtido { get; set; }
        public decimal pendiente { get; set; }
        public decimal costoPromedio { get; set; }
        public bool flagBloquearPartidaSurtida { get; set; }
        public decimal colocada { get; set; }

        public decimal cantidadPendiente { get; set; }
        public string unidad { get; set; }

        public int compras_req { get; set; }
        public bool flagPartidaComprada { get; set; }
        public bool flagBloquearPartidaEntrada { get; set; }

        public int proveedorDistinto { get; set; }
        public string proveedorDistintoDesc { get; set; }
        public string PERU_proveedor { get; set; }

        public string inventariado { get; set; }
        public string noEconomico { get; set; }
    }
}
