using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class BarrenadoraMapping : EntityTypeConfiguration<tblB_Barrenadora>
    {
        public BarrenadoraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.piezasAsignadas).HasColumnName("piezasAsignadas");
            Property(x => x.operadoresAsignados).HasColumnName("operadoresAsignados");
            Property(x => x.activa).HasColumnName("activa");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            HasRequired(x => x.maquina).WithMany().HasForeignKey(x => x.maquinaID);
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblB_Barrenadora");
        }
    }
}
