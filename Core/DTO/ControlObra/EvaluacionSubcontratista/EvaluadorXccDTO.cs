using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class evaluadorXccDTO
    {
        public int id { get; set; }
        public int evaluador { get; set; }
        public string nombreEvaluador { get; set; }
        public string rfc { get; set; }
        public string cc { get; set; }
        public string lstElem { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> lstElementos { get; set; }
        public List<string> lstCC2 { get; set; }
        public List<string> lstElementos2 { get; set; }
        
        public bool esActivo { get; set; }


        public int estatus { get; set; }
        public string mensaje { get; set; }
    }
}
