using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class NominaCCDTO
    {
        public int id { get; set; }
        public DateTime periodoInicial { get; set; }
        public DateTime periodoFinal { get; set; }
        public decimal nominaSemanal { get; set; }
        public string archivo { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }
        public bool isVerificado { get; set; }
        public List<NominaCCProyectosDTO> proyectos { get; set; }
        public string proyectosString { get; set; }
        public decimal totalHH { get; set; }
        public string lblVerificado { get; set; }
    }
}
