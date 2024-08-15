using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.FileManager
{
    public class EnvioDocumentoMapping : EntityTypeConfiguration<tblFM_EnvioDocumento>
    {
        public EnvioDocumentoMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoDocumento).HasColumnName("tipoDocumento");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.documentoID).HasColumnName("documentoID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.fecha).HasColumnName("fecha");
            ToTable("tblFM_EnvioDocumento");
        }
    }
}
