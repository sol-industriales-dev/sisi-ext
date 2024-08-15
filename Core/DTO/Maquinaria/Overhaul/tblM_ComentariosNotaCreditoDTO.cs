using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class tblM_ComentariosNotaCreditoDTO
    {

        public int id { get; set; }
        public int notaCreditoID { get; set; }
        public string comentario { get; set; }
        public string usuarioNombre { get; set; }
        public int usuarioID { get; set; }
        public string fecha { get; set; }
        public string factura { get; set; }
        public int tipoComentario { get; set; }
        public bool tieneEvidencia { get; set; }
        public string nombreArchivo { get; set; }
    }
}
