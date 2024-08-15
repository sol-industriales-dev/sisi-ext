using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class CheckListInfoDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public List<string> ccs { get; set; }
        public int area { get; set; }
        public List<int> lideres { get; set; }
        public int cincoS_clasificar { get; set; }
        public int cincoS_orden { get; set; }
        public int cincoS_limpieza { get; set; }
        public int cincoS_estandarizacion { get; set; }
        public int cincoS_disciplina { get; set; }
        public int cincoS_total { get; set; }
        public List<InspeccionesDTO> inspecciones { get; set; }
    }
}
