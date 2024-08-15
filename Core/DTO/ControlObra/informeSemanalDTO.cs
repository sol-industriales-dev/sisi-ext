using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class informeSemanalDTO
    {
        public int id { get; set; }
        public int plantilla_id { get; set; }
        public string cc { get; set; }
        public string cc_desc { get; set; }
        public DateTime fecha { get; set; }
        public string periodo { get; set; }
        public string fecha_st { get; set; }
        public string division_desc { get; set; }
        public bool estatus { get; set; }
      
    }
}
