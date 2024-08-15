using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class ProveedorEKDTO
    {
        public int numpro { get; set; }
        public string nomcorto { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public string responsable { get; set; }
        public string telefono1 { get; set; }
        public string telefono2 { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string rfc { get; set; }
        public string moneda { get; set; }
        public string cancelado { get; set; }
    }
}
