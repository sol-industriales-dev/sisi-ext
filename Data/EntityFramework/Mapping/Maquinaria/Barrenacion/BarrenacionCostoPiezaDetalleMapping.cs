using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class BarrenacionCostoPiezaDetalleMapping : EntityTypeConfiguration<tblB_BarrenacionCostoPiezaDetalle>
    {
        public BarrenacionCostoPiezaDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.precioUnitarioPieza).HasColumnName("precioUnitarioPieza");
            Property(x => x.cantidadPieza).HasColumnName("cantidadPieza");
            Property(x => x.totalPieza).HasColumnName("totalPieza");
            Property(x => x.activa).HasColumnName("activa");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.idBarrenacionCosto).HasColumnName("idBarrenacionCosto");
            Property(x => x.idPieza).HasColumnName("idPieza");
            HasRequired(x => x.BarrenacionCosto).WithMany().HasForeignKey(y => y.idBarrenacionCosto);
            HasRequired(x => x.pieza).WithMany().HasForeignKey(y => y.idPieza);
            ToTable("tblB_BarrenacionCostoPiezaDetalle");
        }
    }
}
