using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class UsuarioMAZDADTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombreCompleto { get; set; }
        public string correo { get; set; }
        public string nombreUsuario { get; set; }
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }
        public int nivel { get; set; }
        public int orden { get; set; }
    }
}
