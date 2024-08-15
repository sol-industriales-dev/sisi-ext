using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.GestorCorporativo
{
    public class PermisosDTO
    {
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeCrear { get; set; }
        public int usuarioID { get; set; }
        public string nombreUsuario { get; set; }
    }
}
