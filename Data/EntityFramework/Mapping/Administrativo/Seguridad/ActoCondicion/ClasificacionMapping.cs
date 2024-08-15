using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.ActoCondicion
{
    public class ClasificacionMapping : EntityTypeConfiguration<tblSAC_Clasificacion>
    {
        public ClasificacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoRiesgo).HasColumnName("tipoRiesgo");
            ToTable("tblSAC_Clasificacion");
        }
    }
}
