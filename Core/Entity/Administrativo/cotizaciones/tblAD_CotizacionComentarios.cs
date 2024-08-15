using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.cotizaciones
{
    public class tblAD_CotizacionComentarios
    {
        public int id { get; set; }
        public int cotizacionID { get; set; }
        public string comentario { get; set; }
        public string usuarioNombre { get; set; }
        public int usuarioID { get; set; }
        public DateTime fecha { get; set; }
        public string tipo { get; set; }
        public virtual tblAD_Cotizaciones cotizacion{ get; set; }
        public string adjuntoNombre { get; set; }
        public string adjuntoExt { get; set; }
        public byte[] adjunto { get; set; }
    }
}
