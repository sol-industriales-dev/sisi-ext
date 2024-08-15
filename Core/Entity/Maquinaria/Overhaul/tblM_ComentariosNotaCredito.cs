using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_ComentariosNotaCredito
    {
        public int id { get; set; }
        public int notaCreditoID { get; set; }
        public string comentario { get; set; }
        public string usuarioNombre { get; set; }
        public int usuarioID { get; set; }
        public DateTime fecha { get; set; }
        public string factura { get; set; }
        public int tipoComentario { get; set; }
        public string nombreEvidencia { get; set; }
    }
}
