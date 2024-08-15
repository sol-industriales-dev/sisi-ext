using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.FileManager;

namespace Core.Entity.FileManager
{
    public class tblFM_PermisoEspecial
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public TipoPermisoEnum tipoPermiso { get; set; }
        public int entidadID { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }
        public int usuarioCreadorID { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}