using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Enkontrol.OrdenCompra;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class RequisicionDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }    
        public DateTime fecha { get; set; }
        public int libre_abordo { get; set; }
        public string tipo_req_oc { get; set; }
        public int solicito { get; set; }
        public int vobo { get; set; }
        public int autorizo { get; set; }
        public string comentarios { get; set; }
        public string st_estatus { get; set; }
        public string st_impresa { get; set; }
        public string st_autoriza { get; set; }
        public int? emp_autoriza { get; set; }
        public int? empleado_modifica { get; set; }
        public Nullable<DateTime> fecha_modifica { get; set; }
        public string fecha_modificaString { get; set; }
        public Nullable<DateTime> hora_modifica { get; set; }
        public string hora_modificaString { get; set; }
        public Nullable<DateTime> fecha_autoriza { get; set; }
        public int? tmc { get; set; }
        public int? autoriza_activos { get; set; }
        public int? num_vobo { get; set; }
        public int almacen { get; set; }
        public string almacenLAB { get; set; }
        public string ccDescripcion { get; set; }
        public List<RequisicionDetDTO> partidas { get; set; }

        public int almacenDestinoID { get; set; }

        public CuadroComparativoDTO cuadroComparativo { get; set; }
        public bool? consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }
        public string estatusSurtido { get; set; }
        public bool flagCancelar { get; set; }
        public bool? validadoAlmacen { get; set; }
        public bool? validadoRequisitor { get; set; }
        public int? comprador { get; set; }
        public string nombreComprador { get; set; }
        public int proveedor { get; set; }
        public string proveedorDesc { get; set; }
        public bool flagTieneEntrada { get; set; }
        public string solicitoNom { get; set; }
        public string empModificaNom { get; set; }
        public string voboNom { get; set; }
        public string empAutNom { get; set; }
        public string folioOrigen { get; set; }
        public DateTime? fechaSurtidoCompromiso { get; set; }
        public string fechaSurtidoCompromisoString { get; set; }
        public int numeroOC { get; set; }
        public string numeroOCString { get; set; }
        public int usuarioSolicita { get; set; }
        public string usuarioSolicitaDesc { get; set; }
        public string usuarioSolicitaUso { get; set; }
        public int usuarioSolicitaEmpresa { get; set; }
        public string estatus { get; set; }
        public string compradorDesc { get; set; }

        public int insumo { get; set; }
        public decimal precio { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuentaDesc { get; set; }

        public string fechaAutorizacionString { get; set; }
        public string fechaEntregaString { get; set; }
        public string PERU_proveedor { get; set; }
        public string PERU_tipoCompra { get; set; }
    }
}
