using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class ManoObraMapping : EntityTypeConfiguration<tblB_ManoObra>
    {
        public ManoObraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.tipoOperador).HasColumnName("tipoOperador");
            Property(x => x.turno).HasColumnName("turno");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.fechaBaja).HasColumnName("fechaBaja");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.barrenadoraID).HasColumnName("barrenadoraID");
            HasRequired(x => x.barrenadora).WithMany().HasForeignKey(x => x.barrenadoraID);
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.sueldo).HasColumnName("sueldo");
            Property(x => x.jornada).HasColumnName("jornada");
            Property(x => x.fsr).HasColumnName("fsr");
            ToTable("tblB_ManoObra");
        }
    }
}
