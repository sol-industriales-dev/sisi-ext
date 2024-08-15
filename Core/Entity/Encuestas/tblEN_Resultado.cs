using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Resultado
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public virtual tblEN_Encuesta encuesta { get; set; }
        public int preguntaID { get; set; }
        public virtual tblEN_Preguntas pregunta { get; set; }
        public int usuarioRespondioID { get; set; }
        public virtual tblP_Usuario usuarioRespondio { get; set; }
        public int encuestaUsuarioID { get; set; }
        public virtual tblEN_Encuesta_Usuario encuestaUsuario { get; set; }
        public decimal calificacion { get; set; }        
        public DateTime fecha { get; set; }
        public string respuesta { get; set; }
        public decimal? porcentaje { get; set; }
    }
}
