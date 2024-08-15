using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class SeguimientoRequisicionDTO
    {
        public int reqId { get; set; }
        public string reqCc { get; set; }
        public int reqNumero { get; set; }
        public string reqPeruTipoRequisicion { get; set; }
        public int reqSolicito { get; set; }
        public int reqComprador { get; set; }
        public DateTime reqFecha { get; set; }
        public DateTime reqAutoriza { get; set; }
        public int reqLibreAbordo { get; set; }
        public int reqDetInsumo { get; set; }
        public decimal reqDetCantidadOrdenada { get; set; }
        public int? ocId { get; set; }
        public string ocCc { get; set; }
        public int? ocNumero { get; set; }
        public string ocPeruProveedor { get; set; }
        public DateTime? ocFecha { get; set; }
        public DateTime? ocFechaAutoriza { get; set; }
        public string ocBienesServicios { get; set; }
        public DateTime? ocDetFechaEntrega { get; set; }
        public string solicitoNombreCompleto { get; set; }
        public string provedorNombre { get; set; }
        public string insumoNombre { get; set; }
        public string compradorUsuario { get; set; }
        public string compradorSugerido { get; set; }
        public bool? reqValidadoAlmacen { get; set; }
        public bool reqValidadoCompras { get; set; }
        public DateTime? reqFechaValidacionAlmacen { get; set; }
        public bool? reqConsigna { get; set; }
        public bool reqLicitacion { get; set; }
        public bool reqCrc { get; set; }
        public bool reqConvenio { get; set; }
        public int ocTiempoEntrega { get; set; }
        public string ocTiempoEntregaComentarios { get; set; }
        public bool ocColocada { get; set; }
        public DateTime? ocColocadaFecha { get; set; }
        public DateTime? fechaEntrada { get; set; }
    }
}
