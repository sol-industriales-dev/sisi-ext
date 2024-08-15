using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.FileManager
{
    public class TipoArchivoMapping : EntityTypeConfiguration<tblFM_TipoArchivo>
    {
        public TipoArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.nombreLibre).HasColumnName("nombreLibre");
            ToTable("tblFM_TipoArchivo");
        }
    }
}
