using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class GraficaDTO
    {
        public List<string> categorias { get; set; }
        public string serie1Descripcion { get; set; }
        public List<decimal> serie1 { get; set; }
        public List<string> serie1String { get; set; }
        public string serie2Descripcion { get; set; }
        public List<decimal> serie2 { get; set; }
        public List<string> serie2String { get; set; }
        public string serie3Descripcion { get; set; }
        public List<decimal> serie3 { get; set; }
        public List<string> serie3String { get; set; }

        public GraficaDTO()
        {
            categorias = new List<string>();
            serie1 = new List<decimal>();
            serie2 = new List<decimal>();
            serie3 = new List<decimal>();
            serie1String = new List<string>();
            serie2String = new List<string>();
            serie3String = new List<string>();
        }
    }
}
