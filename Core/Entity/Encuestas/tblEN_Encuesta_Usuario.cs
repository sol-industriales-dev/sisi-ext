using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Encuesta_Usuario
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public virtual tblEN_Encuesta encuesta { get; set; }
        public int usuarioResponderID { get; set; }
        public virtual tblP_Usuario usuarioResponder { get; set; }
        public DateTime fecha { get; set; }
        public string comentario { get; set; }
        public bool estatus { get; set; }
        public string asunto { get; set; }
        public bool? telefonica { get; set; }
        public int? usuarioTelefonoID { get; set; }
        public decimal? calificacion { get; set; }
        public string rutaArchivo { get; set; }
        public int tipoRespuesta { get; set; }
    }
}
