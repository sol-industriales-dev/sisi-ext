using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class EncuestaDTO
    {
        public int id { get; set; }
        public int encuestaUsuarioID { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int creadorID { get; set; }
        public string creadorNombre { get; set; }
        public int tipo { get; set; }
        public List<PreguntaDTO> preguntas { get; set; }
        public string fecha { get; set; }
        public string comentario { get; set; }
        public bool contestada { get; set; }

        public string envio { get; set; }
        public string respondio { get; set; }
        public string fechaEnvio { get; set; }
        public string fechaRespondio { get; set; }
        public string departamento { get; set; }
        public string asunto { get; set; }
        public string calificacionEncuesta { get; set; }

        public int departamentoID { get; set; }
        public bool? editar { get; set; }
        public bool? enviar { get; set; }
        public bool? telefonica { get; set; }
        public bool? notificacion { get; set; }
        public bool soloLectura { get; set; }
        public bool? papel { get; set; }
    }
}
