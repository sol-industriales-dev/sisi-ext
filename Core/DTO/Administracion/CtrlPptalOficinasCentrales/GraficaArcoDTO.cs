using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class GraficaArcoDTO
    {
        public List<serieArcoDTO> serie { get; set; }

        public GraficaArcoDTO()
        {
            serie = new List<serieArcoDTO>();
        }
    }

    public class serieArcoDTO
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string color { get; set; }
    }
}
