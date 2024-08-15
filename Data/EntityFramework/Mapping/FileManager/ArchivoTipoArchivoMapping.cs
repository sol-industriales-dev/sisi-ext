using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.FileManager
{
    public class ArchivoTipoArchivoMapping : EntityTypeConfiguration<tblFM_ArchivotblFM_TipoArchivo>
    {

        public ArchivoTipoArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.archivoID).HasColumnName("archivoID");
            Property(x => x.tipoArchivoID).HasColumnName("tipoArchivoID");
            HasRequired(x => x.tipoArchivo).WithMany(x => x.listaArchivos).HasForeignKey(d => d.tipoArchivoID);
            ToTable("tblFM_ArchivotblFM_TipoArchivo");
        }

    }
}
