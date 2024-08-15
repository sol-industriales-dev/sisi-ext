using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class ComentarioDTO
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public string comentario { get; set; }
        public int idMes { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public string fecha { get; set; }
        public string usuarioNombre { get; set; }
        public int idConcepto { get; set; }
    }
}
