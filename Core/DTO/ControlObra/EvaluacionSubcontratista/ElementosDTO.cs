using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class ElementosDTO
    {
        public int id { get; set; }
        public string idbutton { get; set; }
        public string idsection { get; set; }
        public int orden { get; set; }
        public string toltips { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
        public bool SubContratista { get; set; }
        public bool important { get; set; }
        public int idEvaluador { get; set; }
        public bool Aparece { get; set; }
        public List<tblCO_ADP_EvaluacionReq> lstRequerimientos { get; set; }


        public int idSubContratista { get; set; }
        public bool Preguntar { get; set; }
        public int idReq { get; set; }
        public int idAsignacion { get; set; }
    }
}
