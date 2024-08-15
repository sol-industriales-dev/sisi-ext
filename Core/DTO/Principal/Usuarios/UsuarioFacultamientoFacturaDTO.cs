using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class UsuarioFacultamientoFacturaDTO
    {
        public bool bloqueo { get; set; }
        public List<GastosProveedorDTO> listaFacturasPendientes { get; set; }
        public string stringFacturasPendientes { get; set; }
    }
}
