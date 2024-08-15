using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class SubFasesDTO
    {
        public int id { get; set; }
        public string subFase { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int faseID { get; set; }
        public string fase { get; set; }
        public int proyectoID { get; set; }
        public string proyecto { get; set; }
    }
}
