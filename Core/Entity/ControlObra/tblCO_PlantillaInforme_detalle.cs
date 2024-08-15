using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Core.Entity.ControlObra
{
    public class tblCO_PlantillaInforme_detalle
    {
        public long id { get; set; }
        public int ordenDiapositiva { get; set; }
        public string tituloDiapositiva { get; set; }
        [AllowHtml]
        public string contenido { get; set; }

        public int plantilla_id { get; set; }
        public virtual tblCO_PlantillaInforme plantilla { get; set; }
    }
}
