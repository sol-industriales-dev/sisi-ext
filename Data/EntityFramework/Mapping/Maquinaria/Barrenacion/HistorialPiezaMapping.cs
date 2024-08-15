using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class HistorialPiezaMapping : EntityTypeConfiguration<tblB_HistorialPieza>
    {
        public HistorialPiezaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.piezaID).HasColumnName("piezaID");
            HasRequired(x => x.pieza).WithMany().HasForeignKey(x => x.piezaID);
            Property(x => x.horasAcumuladas).HasColumnName("horasAcumuladas");
            Property(x => x.barrenadoraID).HasColumnName("barrenadoraID");
            HasRequired(x => x.barrenadora).WithMany().HasForeignKey(x => x.barrenadoraID);
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.tipoMovimiento).HasColumnName("tipoMovimiento");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioID);
            Property(x => x.precio).HasColumnName("precio");

            ToTable("tblB_HistorialPieza");
        }
    }
}
