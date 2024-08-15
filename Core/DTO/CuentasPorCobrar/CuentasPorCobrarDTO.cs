using Core.DTO.Facturacion;
using Core.Entity.CuentasPorCobrar;
using Core.Enum.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.CuentasPorCobrar
{
    public class CuentasPorCobrarDTO
    {
        public int id { get; set; }
        public int numcte { get; set; }
        public string nombreCliente { get; set; }
        public string cc { get; set; }
        public string idFactura { get; set; }
        public decimal monto { get; set; }
        public decimal montoNuevo { get; set; }
        public DateTime fechaOriginal { get; set; }
        public DateTime fechaNueva { get; set; }
        public string comentarios { get; set; }
        public bool esAutorizar { get; set; }
        public bool esPagado { get; set; }
        public int? autoriza { get; set; }
        public EstatusConvenioEnum estatus { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
        public List<ConvenioDetDTO> lstAbonos { get; set; }


        //
        public string ccDescripcion { get; set; }
        public int usuarioActual { get; set; }
        public DateTime fechaCorte { get; set; }
    }
}
