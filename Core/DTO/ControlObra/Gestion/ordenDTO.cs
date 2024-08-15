using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class ordenDTO
    {
        public ordenesCambioDTO ordenCambio { get; set; }
        public soportesEviDTO lstSoportesEvidencia { get; set; }
        public List<montosDTO> lstMontos { get; set; }
        public List<firmasDTO> LstFirmas { get; set; }
        public string rutaFirmaSub { get; set; }
        public string rutaFirmaConstruplan { get; set; }
    }
}
