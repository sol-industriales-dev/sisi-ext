using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class BarrenacionCostoOtroDetalleMapping : EntityTypeConfiguration<tblB_BarrenacionCostoOtroDetalle>
    {
        public BarrenacionCostoOtroDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.conceptoOtro).HasColumnName("manoObra");
            Property(x => x.precioUnitarioOtro).HasColumnName("costoRenta");
            Property(x => x.cantidadOtro).HasColumnName("diesel");
            Property(x => x.totalOtro).HasColumnName("totalOtro");
            Property(x => x.activa).HasColumnName("activa");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.idBarrenacionCosto).HasColumnName("idBarrenacionCosto");
            HasRequired(x => x.BarrenacionCosto).WithMany().HasForeignKey(y => y.idBarrenacionCosto);
            ToTable("tblB_BarrenacionCostoOtroDetalle");
        }
    }
}
