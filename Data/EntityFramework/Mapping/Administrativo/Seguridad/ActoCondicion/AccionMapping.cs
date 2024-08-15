using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.ActoCondicion
{
    public class AccionMapping : EntityTypeConfiguration<tblSAC_Accion>
    {
        public AccionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            ToTable("tblSAC_Accion");
        }
    }
}
