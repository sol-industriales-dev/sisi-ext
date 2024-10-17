using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class RelFacturaDTO
    {
        public string Folio { get; set; }
        public string Serie { get; set; }
        public string CC { get; set; }
        public int NumOC { get; set; }
        public HttpPostedFileBase XmlFile { get; set; } // Aquí se recibirá el archivo XML
        public decimal Total { get; set; }
        public string Ruta { get; set; }
    }
}
