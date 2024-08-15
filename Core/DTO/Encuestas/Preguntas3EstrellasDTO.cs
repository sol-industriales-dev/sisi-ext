using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class Preguntas3EstrellasDTO
    {
        public int usuarioEnvioID { get; set; }
        public string usuarioEnvioNombre { get; set; }
        public string Encuesta { get; set; }
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
        public int Calificación { get; set; }
        public string Respondio { get; set; }
        public string Fecha { get; set; }
        public string asunto { get; set; }
        public string Proyecto { get; set; }
    }
}
