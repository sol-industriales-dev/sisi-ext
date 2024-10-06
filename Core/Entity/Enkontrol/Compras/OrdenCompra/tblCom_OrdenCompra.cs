using Core.Enum.Enkontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_OrdenCompra
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int? idLibreAbordo { get; set; }
        public string tipo_oc_req { get; set; }
        public int compradorSIGOPLAN { get; set; }
        public int compradorEnkontrol { get; set; }
        public int compradorStarsoft { get; set; }
        public int moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal sub_total_rec { get; set; }
        public decimal iva_rec { get; set; }
        public decimal total_rec { get; set; }
        public string estatus { get; set; }
        public string comentarios { get; set; }
        public string bienes_servicios { get; set; }
        public string CFDI { get; set; }
        public int tiempoEntregaDias { get; set; }
        public string tiempoEntregaComentarios { get; set; }
        public bool anticipo { get; set; }
        public decimal totalAnticipo { get; set; }
        public bool estatusRegistro { get; set; }
        public bool colocada { get; set; }
        public DateTime? colocadaFecha { get; set; }
        public string correoProveedor { get; set; }
        public int? proveedor { get; set; }
        public string st_impresa { get; set; }
        public int autorizo { get; set; }
        public int usuario_autoriza { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public string ST_OC { get; set; }
        public int empleado_autoriza { get; set; }
        public int empleadoUltimaAccion { get; set; }
        public string PERU_proveedor { get; set; }
        public string PERU_cuentaCorriente { get; set; }
        public string PERU_formaPago { get; set; }
        public string PERU_tipoCambio { get; set; }
        public string PERU_tipoCompra { get; set; }
        public DateTime? fechaUltimaAccion { get; set; }
        public TipoUltimaAccionEnum tipoUltimaAccion { get; set; }
        public virtual List<tblCom_OCRetenciones> retenciones { get; set; }
        public virtual List<tblAlm_Movimientos> OC_Movimientos { get; set; }
        public int vobo { get; set; }
        public int vobo2 { get; set; }
        public bool autoriza_activos { get; set; }
        public int solicito { get; set; }
        public string embarquese { get; set; }
        public string concepto_factura { get; set; }
        public string bit_autorecepcion { get; set; }
        public int? almacen_autorecepcion { get; set; }
        public string almacenRecepNom { get; set; }
        public int? empleado_autorecepcion { get; set; }
        public decimal rentencion_antes_iva { get; set; }
        public decimal rentencion_despues_iva { get; set; }
        public int num_requisicion { get; set; }
    }
}
