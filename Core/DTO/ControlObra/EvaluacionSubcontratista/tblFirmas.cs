using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class tblFirmas
    {
        public int id { get; set; }
        public int idOrdenDeCambio { get; set; }
        public string firma { get; set; }
        public int idFirma { get; set; }
        public string firmaDigital { get; set; }
        public int idRow { get; set; }
        public bool Autorizando { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public bool Estado { get; set; }
    }
}
