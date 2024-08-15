using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class DetalleCapturaMapping : EntityTypeConfiguration<tblB_DetalleCaptura>
    {
        public DetalleCapturaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.bordo).HasColumnName("bordo");
            Property(x => x.espaciamiento).HasColumnName("espaciamiento");
            Property(x => x.barrenos).HasColumnName("barrenos");
            Property(x => x.profundidad).HasColumnName("profundidad");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.densidadMaterial).HasColumnName("densidadMaterial");
            Property(x => x.tipoBarreno).HasColumnName("tipoBarreno");
            Property(x => x.subbarreno).HasColumnName("subbarreno");
            Property(x => x.capturaID).HasColumnName("capturaID");
            HasRequired(x => x.captura).WithMany(x => x.detalles).HasForeignKey(d => d.capturaID);
            ToTable("tblB_DetalleCaptura");
        }
    }
}
