using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Kubrix
{
    public class BusqKubrixDTO
    {
        public List<string> obra { get; set; }
        public List<string> ccEnkontrol { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string economico { get; set; }
        public List<int> lstModelo { get; set; }
        public int tipoEquipo { get; set; }
        public int tipoEquipoMayor { get; set; }
        public List<string> maquinas { get; set; }
        public int tipoIntervalo { get; set; }
        public int? idDivision { get; set; }
    }
}