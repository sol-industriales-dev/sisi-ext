using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class DivicionesMenuDTO
    {
        public int idPlantilla { get; set; }
        public string contratos { get; set; }
        public string nombrePlantilla{ get; set; }
        
        public int id { get; set; }
        public int idEvaluador { get; set; }
        public string idbutton { get; set; }
        public string idsection { get; set; }
        public int orden { get; set; }
        public string toltips { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
        public int estatus { get; set; }
        public string mensaje { get; set; }
        public bool important { get; set; }
        public List<RequerimientosDTO> lstRequerimientos { get; set; }
        public bool estaEvaluado { get; set; }
    }
}
