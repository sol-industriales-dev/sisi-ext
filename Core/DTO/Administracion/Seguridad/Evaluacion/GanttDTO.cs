using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Evaluacion
{
    public class GanttDTO
    {
        public int id { get; set; }
        public string text { get; set; }
        public string start_date { get; set; }
        public int duration { get; set; }
        public int? parent { get; set; }
        public decimal progress { get; set; }
        public bool open { get; set; }
        public List<string> users { get; set; }
        public int priority { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }
        public string progressColor { get; set; }
    }
}
