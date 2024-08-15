using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class BarrenacionCostoMapping : EntityTypeConfiguration<tblB_BarrenacionCosto>
    {
        public BarrenacionCostoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.manoObra).HasColumnName("manoObra");
            Property(x => x.costoRenta).HasColumnName("costoRenta");
            Property(x => x.diesel).HasColumnName("diesel");
            Property(x => x.totalCosto).HasColumnName("totalCosto");
            Property(x => x.activa).HasColumnName("activa");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblB_BarrenacionCosto");
        }
    }
}
