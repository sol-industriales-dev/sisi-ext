using Core.Entity.Principal.Multiempresa;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Principal.Multiempresa
{
    public class SubdivisionMapping : EntityTypeConfiguration<tblP_Subdivision>
    {
        public SubdivisionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.abreviacion).HasColumnName("abreviacion");
            Property(x => x.divisionID).HasColumnName("divisionID");
            HasRequired(x => x.division).WithMany(x => x.subdivisiones).HasForeignKey(d => d.divisionID);
            Property(x => x.activo).HasColumnName("activo");
            ToTable("tblP_Division");
        }
    }
}
