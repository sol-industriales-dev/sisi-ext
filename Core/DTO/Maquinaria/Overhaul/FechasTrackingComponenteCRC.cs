using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class FechasTrackingComponenteCRC
    {
        public string fechaEnvio { get; set; }
        public string fechaRecepcion { get; set; }
        public string fechaCotizacion { get; set; }
        public string claveCotizacion { get; set; }
        public string costo { get; set; }
        public string parcial { get; set; }
        public string fechaAutorizacion { get; set; }
        public string fechaRequisicion { get; set; }
        public string folioRequisicion { get; set; }
        public string fechaEnvioOC { get; set; }
        public string OC { get; set; }        
        public string fechaTerminacion { get; set; }
        public string fechaRecoleccion { get; set; }
        public string almacen { get; set; }
        public string folioFactura { get; set; }
        public int notaCredito { get; set; }
        public string comprador { get; set; }
        public DateTime? entradaAlmacen { get; set; }
    }
}