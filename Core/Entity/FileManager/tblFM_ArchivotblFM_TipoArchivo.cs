
namespace Core.Entity.FileManager
{
    public class tblFM_ArchivotblFM_TipoArchivo
    {
        public long id { get; set; }
        public long archivoID { get; set; }
        public int tipoArchivoID { get; set; }
        public virtual tblFM_TipoArchivo tipoArchivo { get; set; }
    }
}
