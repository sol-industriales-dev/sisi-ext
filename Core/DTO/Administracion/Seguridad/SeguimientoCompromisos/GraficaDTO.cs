using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoCompromisos
{
    public class GraficaDTO
    {
        public List<string> categorias { get; set; }
        public string serie1Descripcion { get; set; }
        public List<decimal> serie1 { get; set; }
        public string serie2Descripcion { get; set; }
        public List<decimal> serie2 { get; set; }

        public GraficaDTO()
        {
            categorias = new List<string>();
            serie1 = new List<decimal>();
            serie2 = new List<decimal>();
        }
    }
}
