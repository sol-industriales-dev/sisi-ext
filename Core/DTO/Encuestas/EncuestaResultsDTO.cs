using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class EncuestaResultsDTO
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public string encuestaNombre { get; set; }
        public int usuarioEnvioID { get; set; }
        public string usuarioEnvioNombre { get; set; }
        public int usuarioResponderID { get; set; }
        public string usuarioResponderNombre { get; set; }
        public string fecha { get; set; }
        public string fechaRespndio { get; set; }
        public DateTime fechaRespndioValue { get; set; }
        public string comentario { get; set; }
        public string ver { get; set; }
        public string asunto { get; set; }
        public string calificacion { get; set; }

        public string calificacionPorcentajePromedio { get; set; }
        public string Proyecto { get; set; }

        public string descarga { get; set; }

        public int tipoRespuesta { get; set; }
        public string tipoRespuestaDesc { get; set; }
    }
}
