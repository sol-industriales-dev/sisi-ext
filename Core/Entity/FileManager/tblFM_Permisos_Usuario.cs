using Core.Entity.Principal.Usuarios;
using Core.Enum.FileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.FileManager
{
    public class tblFM_Permisos_Usuario
    {
        #region SQL
        public long id { get; set; }
        public int usuarioID { get; set; }
        public long archivoID { get; set; }
        public TipoPermisoEnum tipoPermiso { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }
        public int estatusVista { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public virtual tblP_Usuario usuario { get; set; }
        public virtual tblFM_Archivo archivo { get; set; }
        #endregion
    }
}
