using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class DetalleAutorizacionDTO
    {
        public int numero { get; set; }
        public string nombre { get; set; }
        public string tipoAutorizante { get; set; }
        public bool estatus { get; set; }
        public bool rechazado { get; set; }
    }
}
