using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Enum.Enkontrol;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_Req
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int idLibreAbordo { get; set; }
        public int idTipoReqOc { get; set; }
        public int solicito { get; set; }
        public int vobo { get; set; }
        public int autorizo { get; set; }
        public string comentarios { get; set; }
        public string stEstatus { get; set; }
        public bool stImpresa { get; set; }
        public bool stAutoriza { get; set; }
        public int empAutoriza { get; set; }
        public int empModifica { get; set; }
        public DateTime modifica { get; set; }
        public DateTime autoriza { get; set; }
        public bool isTmc { get; set; }
        public bool isActivos { get; set; }
        public int numVobo { get; set; }
        public string folioAsignado { get; set; }
        public bool? consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }
        public int proveedor { get; set; }
        public bool? validadoAlmacen { get; set; }
        public bool validadoCompras { get; set; }
        public bool? validadoRequisitor { get; set; }
        public DateTime? fechaValidacionAlmacen { get; set; }
        public int? comprador { get; set; }
        public int empleadoUltimaAccion { get; set; }
        public DateTime? fechaUltimaAccion { get; set; }
        public TipoUltimaAccionEnum tipoUltimaAccion { get; set; }
        public DateTime? fechaSurtidoCompromiso { get; set; }
        public DateTime? fechaEnvioCorreoProveedor { get; set; }
        public int usuarioSolicita { get; set; }
        public string usuarioSolicitaUso { get; set; }
        public int usuarioSolicitaEmpresa { get; set; }
        public string PERU_tipoRequisicion { get; set; }
        public string PERU_codigoAuditoria { get; set; }
        public bool estatusRegistro { get; set; }

        public virtual List<tblAlm_Movimientos> req_Movimientos { get; set; }

        #region Backlog
        [NotMapped]
        public int? idBL { get; set; }
        #endregion
    }
}
