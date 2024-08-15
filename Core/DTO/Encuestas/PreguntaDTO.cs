using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class PreguntaDTO
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public int encuestaUsuarioID { get; set; }
        public string pregunta { get; set; }
        public int calificacion { get; set; }
        public string respuesta { get; set; }
        public string estatus { get; set; }
        public int tipo { get; set; }
    }
}
