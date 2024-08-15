using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_ResultadoProveedores
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public virtual tblEN_EncuestaProveedores encuesta { get; set; }
        public int preguntaID { get; set; }
        public virtual tblEN_PreguntasProveedores pregunta { get; set; }
        public int usuarioRespondioID { get; set; }
        public int encuestaFolioID { get; set; }
        public decimal calificacion { get; set; }
        public DateTime fecha { get; set; }
        public int tipoEncuesta { get; set; }
        public string respuesta { get; set; }
        public decimal? porcentaje { get; set; }
        public decimal? calificacionPonderacion { get; set; }

        [ForeignKey("encuestaFolioID")]
        public virtual tblEN_ResultadoProveedoresDet Detalle { get; set; }

        [ForeignKey("usuarioRespondioID")]
        public virtual tblP_Usuario evaluador { get; set; }
    }
}
