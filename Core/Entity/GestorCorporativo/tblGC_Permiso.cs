using Core.Entity.Principal.Usuarios;

namespace Core.Entity.GestorCorporativo
{
    public class tblGC_Permiso
    {
        public long id { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public long archivoID { get; set; }
        public virtual tblGC_Archivo archivo { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeCrear { get; set; }
    }
}
