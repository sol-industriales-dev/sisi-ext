using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class OrdenCompraColombiaDTO
    {
        #region key
        public string cc { get; set; }
        public int numero { get; set; }
        #endregion
        public int idOrdenCompra { get; set; }
        public int id { get; set; }
        public int comprador { get; set; }
        public int proveedor { get; set; }
        public int moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal sub_tot_rec { get; set; }
        public decimal iva_rec { get; set; }
        public decimal total_rec { get; set; }
        public decimal sub_tot_ajus { get; set; }
        public decimal iva_ajus { get; set; }
        public decimal total_ajus { get; set; }
        public string estatus { get; set; }
        public string comentarios { get; set; }
        public int solicito { get; set; }
        public int vobo { get; set; }
        public int autorizo { get; set; }
        public decimal sub_tot_canc { get; set; }
        public decimal iva_canc { get; set; }
        public decimal total_canc { get; set; }
        public decimal total_fac { get; set; }
        public decimal total_pag { get; set; }
        public string embarquese { get; set; }
        public decimal tc_cc { get; set; }
        public string anticipo { get; set; }
        public decimal? monto_anticipo { get; set; }
        public bool anticipoBool { get; set; }
        public decimal totalAnticipo { get; set; }
        public int? almacen { get; set; }
        public string bit_autorecepcion { get; set; }
        public int? almacen_autorecepcion { get; set; }
        public int? empleado_autorecepcion { get; set; }
        public decimal rentencion_antes_iva { get; set; }
        public decimal rentencion_despues_iva { get; set; }
        public string bienes_servicios { get; set; }
        public string concepto_factura { get; set; }
        public decimal tot_fac_ret { get; set; }
        public decimal tot_ret_ret { get; set; }
        public int? usuario_autoriza { get; set; }
        public string imprime_porcentaje { get; set; }
        public string ST_OC { get; set; }
        public string vobo_aut { get; set; }
        public string aut_aut { get; set; }
        public string vobo_informa { get; set; }
        public int vobo2 { get; set; }
        public int vobo3 { get; set; }
        public int vobo4 { get; set; }
        public int vobo5 { get; set; }
        public int vobo6 { get; set; }
        public int cambia_vobo { get; set; }
        public DateTime? fecha_vobo { get; set; }
        public DateTime? fecha_vobo2 { get; set; }
        public DateTime? fecha_vobo3 { get; set; }
        public string bit_af { get; set; }
        public string bit_arrenda { get; set; }
        public string estatus_bloqueo { get; set; }
        public string ruta_pdf { get; set; }
        public string ruta_map { get; set; }
        #region prop de req
        public DateTime fecha { get; set; }
        public int libre_abordo { get; set; }
        public string tipo_req_oc { get; set; }
        public string tipo_oc_req { get; set; }
        public string st_autoriza { get; set; }
        public int? emp_autoriza { get; set; }
        public int? num_vobo { get; set; }
        public string st_estatus { get; set; }
        public string st_impresa { get; set; }
        public int? empleado_modifica { get; set; }
        public DateTime? fecha_modifica { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public string st_autorizada { get; set; }
        public int tmc { get; set; }
        public int autoriza_activos { get; set; }
        public int? empleado_autoriza { get; set; }
        #endregion
        public List<OrdenCompraDetDTO> lstPartidas { get; set; }

        public string proveedorNom { get; set; }
        public string compradorNom { get; set; }
        public string solicitoNom { get; set; }
        public string autorizoNom { get; set; }
        public string almacenRecepNom { get; set; }
        public string empleadoRecepNom { get; set; }
        public List<OrdenCompraRetencionesDTO> lstRetenciones { get; set; }
        public List<OrdenCompraPagosDTO> lstPagos { get; set; }
        public string ccDesc { get; set; }
        public bool flagPuedeAutorizar { get; set; }
        public List<dynamic> lstVobo { get; set; }
        public List<dynamic> lstAutorizaciones { get; set; }
        public bool checkbox { get; set; }
        public bool voboPendiente { get; set; }
        public int numVobos { get; set; }

        public string folioOrigen { get; set; }
        public int ordenTraspaso { get; set; }
        public bool flagPuedeDarVobo { get; set; }

        public bool tieneCuadro { get; set; }
        public int cuadrosExistentes { get; set; }
        public bool consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }
        public bool flagCancelar { get; set; }

        public int numeroRequisicion { get; set; }
        public string CFDI { get; set; }
        public bool flagRequisicionComprada { get; set; }
        public bool flagPuedeGuardar { get; set; }

        public int area { get; set; }
        public int cuenta { get; set; }
        public string inventariado { get; set; }
        public string listComprasString { get; set; }
        public int countCuadroComparativo { get; set; }

        public int tiempoEntregaDias { get; set; }
        public string tiempoEntregaComentarios { get; set; }

        public bool flagTMC { get; set; }
        public bool flagActivoFijo { get; set; }
        public bool colocada { get; set; }
        public DateTime colocadaFecha { get; set; }
        public string correoProveedor { get; set; }
        public bool validadoCompras { get; set; }
        public bool validadoAlmacen { get; set; }

        public bool cuadroGeneradoConCalificacion { get; set; }
        public bool voboProveedorNoOptimo { get; set; }
        public int idVoboProveedorNoOptimo { get; set; }
        public bool puedeDarVoboProvNoOptimo { get; set; }
        public int ultimoMovimiento { get; set; }
        public int remision { get; set; }

        public bool flagValidacionInsumoSubcontratista { get; set; }
        public bool flagCompraReset { get; set; }
        public bool flagCompraResetNoImpresa { get; set; }
        public bool flagCompraInterna { get; set; }
        public bool flagCompraTraspasoAlmacen { get; set; }
        public int categoria { get; set; }

        public string vobosString { get; set; }
        public string autorizacionesString { get; set; }

        public string areaCuentaDesc { get; set; }

        public bool esOC_Interna { get; set; }
        public int compradorSIGOPLAN { get; set; }

        public string stringVobosAutorizaciones { get; set; }
        public string PERU_proveedor { get; set; }
        public string PERU_cuentaCorriente { get; set; }
        public string PERU_formaPago { get; set; }
        public string PERU_tipoCambio { get; set; }
        public string strNumOC { get; set; }
        public string strNumReq { get; set; }
        public bool estatusRegistro { get; set; }
        public int empresa { get; set; }
        public string nombreComprador { get; set; }
        public string PERU_tipoCompra { get; set; }
        public string PERU_guiaCompraPrefijo { get; set; }
        public string PERU_guiaCompraFolio { get; set; }
        public string PERU_tipoDocumento { get; set; }
        public string PERU_folioDocumento { get; set; }

        public bool flagSISUN { get; set; }
    }
}
