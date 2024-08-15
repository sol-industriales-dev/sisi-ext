using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Encuesta_Check_Usuario
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public int usuarioID { get; set; }
        public bool crear { get; set; }
        public bool? ver { get; set; }
        public bool? editar { get; set; }
        public bool? enviar { get; set; }
        public bool? contestaTelefonica { get; set; }
        public bool? recibeNotificacion { get; set; }
        public bool? contestaPapel { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("encuestaID")]
        public virtual tblEN_Encuesta Encuesta { get; set; }
    }
}
