using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.FileManager
{
    public class PermisosGestorDTO
    {
        public int tipoPermiso { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }
        public bool puedeCrearSubdivision { get; set; }
        public bool puedeCargarMultiple { get; set; }

        /// <summary>
        /// 0 (No seleccionada ni parcialmente seleccionada)
        /// 1 (Parcialmente seleccionada)
        /// 2 (Seleccioanda y parcialmente seleccionada)
        /// </summary>
        public int estatusVista { get; set; }

        public int usuarioID { get; set; }
        public string nombreUsuario { get; set; }
    }
}
