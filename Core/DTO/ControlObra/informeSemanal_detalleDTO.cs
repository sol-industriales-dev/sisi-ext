using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.DTO.ControlObra
{
    public class informeSemanal_detalleDTO
    {
        public int id { get; set; }
        public int informe_id { get; set; }
        public int plantilla_id { get; set; }
        public int ordenDiapositiva { get; set; }
        public string tituloDiapositiva { get; set; }
        [AllowHtml]
        public string contenido { get; set; }
        [AllowHtml]
        public string pdf { get; set; }
    }
}
