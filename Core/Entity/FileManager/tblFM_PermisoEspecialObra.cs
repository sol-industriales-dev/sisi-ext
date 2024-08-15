using Core.Enum.FileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.FileManager
{
    public class tblFM_PermisoEspecialObra
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
        [NotMapped]
        public List<tblFM_Archivo> archivos { get; set; }
    }
}
