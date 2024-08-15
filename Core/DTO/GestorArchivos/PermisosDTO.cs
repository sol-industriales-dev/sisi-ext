using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.GestorArchivos
{
    public class PermisosDTO
    {

        public int usuarioID { get; set; }
        public int directorioID { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }

    }
}
