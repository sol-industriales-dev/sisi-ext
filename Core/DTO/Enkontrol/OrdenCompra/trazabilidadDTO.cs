using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class trazabilidadDTO
    {
        //public string cc { get; set; }
        //public int req_numero { get; set; }
        //public DateTime req_fecha { get; set; }
        //public int req_usuario_numero { get; set; }
        //public string req_usuario_nombre { get; set; }
        //public string req_estatus { get; set; }
        //public int req_autorizador_numero { get; set; }
        //public string req_autorizador_nombre { get; set; }
        //public int comp_numero { get; set; }
        //public DateTime comp_fecha { get; set; }
        //public int comp_prov_numero { get; set; }
        //public string comp_prov_nombre { get; set; }
        //public string comp_autorecepcion { get; set; }
        //public string comp_estatus { get; set; }
        //public string como_surtido_estatus { get; set; }
        //public int comp_comprador_numero { get; set; }
        //public string comp_comprador_nombre { get; set; }
        //public bool tiene_entrada { get; set; }
        //public bool tiene_factura { get; set; }
        //public bool tiene_contrarecibo { get; set; }

        public string req_cc { get; set; }
        public string req_ccDesc { get; set; }
        public string req_num { get; set; }
        public DateTime req_fecha { get; set; }
        public string req_usuario { get; set; }
        public string req_estatus { get; set; }
        public string req_autoriza { get; set; }
        public string req_autoriza_fecha { get; set; }
        public string comp_num { get; set; }
        public DateTime comp_fecha { get; set; }
        public int comp_prov_num { get; set; }
        public string comp_prov_nom { get; set; }
        public string comp_estatus { get; set; }
        public string comp_vobo1_nom { get; set; }
        public DateTime comp_vobo1_fecha { get; set; }
        public string comp_vobo2_nom { get; set; }
        public DateTime comp_vobo2_fecha { get; set; }
        public string comp_aut_nom { get; set; }
        public DateTime comp_aut_fecha { get; set; }
        public string comp_surtido { get; set; }
        public string comp_comprador_nom { get; set; }
        public string comp_tiene_entrada { get; set; }
        public string comp_tiene_factura { get; set; }
        public string fac_tiene_contrarecibo { get; set; }
        public string fac_portal_existe { get; set; }
        public string fac_tipo { get; set; }
        public string fac_portal_estatus { get; set; }
        public DateTime fac_portal_fecha { get; set; }
        public DateTime fac_contrarecibo_fecha { get; set; }
        public string fac_numero { get; set; }
        public Decimal comp_total_factura { get; set; }
        public Decimal comp_total_entrada { get; set; }
        public Decimal comp_tipo_cambio { get; set; }
        public string fac_contrarecibo_folio { get; set; }
        public string fac_tiene_pago { get; set; }
        public Decimal fac_pagado { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuentaDesc { get; set; }
    }
}
