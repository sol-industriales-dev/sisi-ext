using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class ReqDetalleComentarioDTO
    {
        public int partida { get; set; }
        public string insumo { get; set; }
        public string comentario { get; set; }
        public DateTime fecha { get; set; }
    }
}
