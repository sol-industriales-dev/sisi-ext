using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class TiemposReparacionDTO
    {
        public string descripcion { get; set; }
        public string noComponente { get; set; }
        public string noEconomico { get; set; }
        public string obra { get; set; }
        public string cotizacion { get; set; }
        public string proveedor { get; set; }
        public string fechaEnvioCRC { get; set; }
        public int trasladoCRC { get; set; }
        public string fechaRecepcionCRC { get; set; }
        public int desarmado { get; set; }
        public string fechaCotizacion { get; set; }
        public int autorizacion { get; set; }
        public string fechaAutorizacion { get; set; }
        public int armado { get; set; }
        public string fechaTerminado { get; set; }
        public int recoleccion { get; set; }
        public string fechaRecoleccion { get; set; }
        public int trasladoAlmacen { get; set; }
        public string fechaEntradaAlmacen { get; set; }
        public int diasCRC { get; set; }
        public int diasProceso { get; set; }
        public int diasReparacion { get; set; }
        public int proveedorID { get; set; }
        public string obraID { get; set; }
        public int estatus { get; set; }
        public string rq { get; set; }
        public string oc { get; set; }
        public string factura { get; set; }
    }
}
