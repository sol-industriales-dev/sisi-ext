using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class VoboDTO
    {
        public int usu_numero { get; set; }
        public string usu_nombre { get; set; }
        public int numVobos { get; set; }
        public int consecutivo { get; set; }
        public string color { get; set; }

        public bool flagCompraSISUN { get; set; }
        public string vobo_aut { get; set; }
    }
}
