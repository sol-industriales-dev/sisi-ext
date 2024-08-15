using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cotizaciones
{
    public class tblAD_CotizacionComentariosDTO
    {
        public int id { get; set; }
        public int cotizacionID { get; set; }
        public string comentario { get; set; }
        public string usuarioNombre { get; set; }
        public int usuarioID { get; set; }
        public string fecha { get; set; }
        public string tipo { get; set; }
        public string adjuntoNombre { get; set; }
    }
}
