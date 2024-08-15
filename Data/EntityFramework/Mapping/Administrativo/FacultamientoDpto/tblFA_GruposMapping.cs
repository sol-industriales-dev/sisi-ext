using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace Data.EntityFramework.Mapping.Administrativo.FacultamientoDpto
{
    public class tblFA_GruposMapping : EntityTypeConfiguration<tblFA_Grupos>
    {
        public tblFA_GruposMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblFA_Grupos");
        }
    }
}
