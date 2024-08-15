using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.FileManager
{
    public class ArchivoMapping : EntityTypeConfiguration<tblFM_Archivo>
    {
        public ArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.padreID).HasColumnName("padreID");
            Property(x => x.nivel).HasColumnName("nivel");
            Property(x => x.año).HasColumnName("año");
            Property(x => x.divisionID).HasColumnName("divisionID");
            Property(x => x.subdivisionID).HasColumnName("subdivisionID");
            Property(x => x.ccID).HasColumnName("ccID");
            Property(x => x.esCarpeta).HasColumnName("esCarpeta");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.perteneceSeguridad).HasColumnName("perteneceSeguridad");
            Property(x => x.tipoCarpeta).HasColumnName("tipoCarpeta");
            ToTable("tblFM_Archivo");
        }
    }
}
