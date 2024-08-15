using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Entity.ControlObra
{
    public class tblCO_informeSemanal_detalle
    {
        public int id { get; set; }
        public int ordenDiapositiva { get; set; }
        public string tituloDiapositiva { get; set; }
        [AllowHtml]
        public string contenido { get; set; }
        [AllowHtml]
        public string pdf { get; set; }

        public int informe_id { get; set; }
        public virtual tblCO_informeSemanal informe { get; set; }
    }
}
