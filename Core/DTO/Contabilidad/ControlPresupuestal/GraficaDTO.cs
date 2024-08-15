using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.ControlPresupuestal
{
    public class GraficaDTO
    {
        public List<string> meses { get; set; }
        public string serie1Descripcion { get; set; }
        public List<decimal> serie1 { get; set; }
        public string serie2Descripcion { get; set; }
        public List<decimal> serie2 { get; set; }
        public string serie3Descripcion { get; set; }
        public List<decimal> serie3 { get; set; }

        public GraficaDTO()
        {
            meses = new List<string>();
            serie1 = new List<decimal>();
            serie2 = new List<decimal>();
            serie3 = new List<decimal>();
        }

        public List<SerieDTO> series { get; set; }
    }
}
