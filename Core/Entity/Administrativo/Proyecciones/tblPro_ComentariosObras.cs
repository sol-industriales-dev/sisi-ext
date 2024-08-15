using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_ComentariosObras
    {
        public int id { get; set; }
        public int capturadeObrasID { get; set; }
        public int registroID { get; set; }
        public DateTime fecha { get; set; }
        public string comentario { get; set; }
        public string usuarioNombre { get; set; }
        public int usuarioID { get; set; }
        public bool estatusComentario { get; set; }

        public string adjuntoNombre { get; set; }
        public string adjuntoExt { get; set; }
        public byte[] adjunto { get; set; }
    }
}
