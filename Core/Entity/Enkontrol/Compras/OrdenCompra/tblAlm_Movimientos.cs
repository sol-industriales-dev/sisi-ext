using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Enkontrol.Compras.Requisicion;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Movimientos
    {
        #region SQL
        public int id { get; set; }
        public int almacen { get; set; }
        public int tipo_mov { get; set; }
        public int numero { get; set; }
        public string cc { get; set; }
        public int compania { get; set; }
        public int periodo { get; set; }
        public int ano { get; set; }
        public int orden_ct { get; set; }
        public int frente { get; set; }
        public DateTime fecha { get; set; }
        public int proveedor { get; set; }
        public decimal total { get; set; }
        public string estatus { get; set; }
        public string transferida { get; set; }
        public int alm_destino { get; set; }
        public string cc_destino { get; set; }
        public string comentarios { get; set; }
        public string tipo_trasp { get; set; }
        public decimal tipo_cambio { get; set; }
        public bool estatusHabilitado { get; set; }
        public string ccReq { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string PERU_proveedor { get; set; }
        public string PERU_guiaCompraPrefijo { get; set; }
        public string PERU_guiaCompraFolio { get; set; }
        public string PERU_tipoDocumento { get; set; }
        public string PERU_folioDocumento { get; set; }
        #endregion

        public int? numeroOC { get; set; }
        public virtual tblCom_OrdenCompra ordenCompra { get; set; }

        public int? numeroReq { get; set; }
        public virtual tblCom_Req requisicion { get; set; }
        public virtual List<tblAlm_MovimientosDet> mov_detalles { get; set; }
    }
}
