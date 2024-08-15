using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Kubrix
{
    public class CapturaMaqDTO
    {
        public string ccObra { get; set; }
        public string fecha { get; set; }
        public string economico { get; set; }
        public string turno { get; set; }
        public string horoInicial { get; set; }
        public string horoFinal { get; set; }
        public string paroClima { get; set; }
        public string hrsMtto { get; set; }
        public string horasTrab { get; set; }
        public string horasProg { get; set; }
        public string horasEfectivas { get; set; }
        public string eficiencia { get; set; }
        public string consumo { get; set; }
        public string grupoEquipo { get; set; }
        public string rendTeorico { get; set; }
        public string rendReal { get; set; }
        public string rendimiento { get; set; }
    }
}
