using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class tblCom_sp_proveedoresConsultaDTO
    {
        public int id { get; set; }
        public decimal numpro { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string rfc { get; set; }
        public bool vobo { get; set; }
        public bool Autorizado { get; set; }
        public bool statusAutorizacion { get; set; }
        public bool puedeDarPrimerVobo { get; set; }
        public bool puedeDarVobo { get; set; }
        public bool puedeAutorizar { get; set; }
        public bool esEnKontrol { get; set; }
        public string persona_fisica { get; set; }
        public string a_nombre { get; set; }
        public string a_paterno { get; set; }
        public string a_materno { get; set; }
        public string descEstatus { get; set; }
    }
}
