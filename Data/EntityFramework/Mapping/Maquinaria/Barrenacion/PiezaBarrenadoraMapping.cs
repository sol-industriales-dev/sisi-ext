using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class PiezaBarrenadoraMapping : EntityTypeConfiguration<tblB_PiezaBarrenadora>
    {
        public PiezaBarrenadoraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.noSerie).HasColumnName("noSerie");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.tipoPieza).HasColumnName("tipoPieza");
            Property(x => x.tipoBroca).HasColumnName("tipoBroca");
            Property(x => x.barraSegunda).HasColumnName("barraSegunda");
            Property(x => x.horasTrabajadas).HasColumnName("horasTrabajadas");
            Property(x => x.horasAcumuladas).HasColumnName("horasAcumuladas");
            Property(x => x.reparando).HasColumnName("reparando");
            Property(x => x.cantidadReparaciones).HasColumnName("cantidadReparaciones");
            Property(x => x.precio).HasColumnName("precio");
            Property(x => x.activa).HasColumnName("activa");
            Property(x => x.montada).HasColumnName("montada");
            Property(x => x.culataID).HasColumnName("culataID");
            Property(x => x.cilindroID).HasColumnName("cilindroID");
            Property(x => x.barrenadoraID).HasColumnName("barrenadoraID");

            ToTable("tblB_PiezaBarrenadora");
        }
    }
}
