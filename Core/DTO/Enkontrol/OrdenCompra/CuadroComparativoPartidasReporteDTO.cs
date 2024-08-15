using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class CuadroComparativoPartidasReporteDTO
    {
        public string partida { get; set; }
        public string insumo { get; set; }
        public string cantidad { get; set; }
        public string precioProv1 { get; set; }
        public string importeProv1 { get; set; }
        public string precioProv2 { get; set; }
        public string importeProv2 { get; set; }
        public string precioProv3 { get; set; }
        public string importeProv3 { get; set; }
        public string ultCompraProveedor { get; set; }
        public string ultCompraFolio { get; set; }
        public string ultCompraFecha { get; set; }
        public string ultCompraPrecio { get; set; }

        public string calificacion1 { get; set; }
        public string calificacion2 { get; set; }
        public string calificacion3 { get; set; }
    }
}
