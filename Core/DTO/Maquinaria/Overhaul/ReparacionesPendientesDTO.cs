using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ReparacionesPendientesDTO
    {
        public string descripcion { get; set; }
        public string noComponente { get; set; }
        public string noEconomico { get; set; }
        public string obra { get; set; }
        public string cotizacion { get; set; }
        public string proveedor { get; set; }
        public string rq { get; set; }
        public string comprador { get; set; }
        public string fecha_rq { get; set; }
        public string oc { get; set; }
        public string factura { get; set; }
        public string fecha_envio { get; set; }
        public string fecha_recepcion { get; set; }
        public string fecha_terminacion { get; set; }
        public string almacen { get; set; }
        public DateTime fecha { get; set; }
        public int proveedorID { get; set; }
        public string obraID { get; set; }
    }
}
