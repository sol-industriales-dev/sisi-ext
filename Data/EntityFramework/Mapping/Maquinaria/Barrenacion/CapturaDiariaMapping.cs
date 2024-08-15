using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class CapturaDiariaMapping : EntityTypeConfiguration<tblB_CapturaDiaria>
    {
        public CapturaDiariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.horometroFinal).HasColumnName("horometroFinal");
            Property(x => x.horasTrabajadas).HasColumnName("horasTrabajadas");
            Property(x => x.tipoCaptura).HasColumnName("tipoCaptura");
            Property(x => x.turno).HasColumnName("turno");
            Property(x => x.claveOperador).HasColumnName("claveOperador");
            Property(x => x.precioOperador).HasColumnName("precioOperador");
            Property(x => x.fsrOperador).HasColumnName("fsrOperador");
            Property(x => x.totalOperador).HasColumnName("totalOperador");
            Property(x => x.claveAyudante).HasColumnName("claveAyudante");
            Property(x => x.precioAyudante).HasColumnName("precioAyudante");
            Property(x => x.fsrAyudante).HasColumnName("fsrAyudante");
            Property(x => x.totalAyudante).HasColumnName("totalAyudante");
            Property(x => x.brocaID).HasColumnName("brocaID");
            Property(x => x.martilloID).HasColumnName("martilloID");
            Property(x => x.barraID).HasColumnName("barraID");
            Property(x => x.barraSegundaID).HasColumnName("barraSegundaID");
            Property(x => x.culataID).HasColumnName("culataID");
            Property(x => x.portabitID).HasColumnName("portabitID");
            Property(x => x.cilindroID).HasColumnName("cilindroID");
            Property(x => x.zancoID).HasColumnName("zancoID");

            Property(x => x.barrenadoraID).HasColumnName("barrenadoraID");
            HasRequired(x => x.barrenadora).WithMany().HasForeignKey(x => x.barrenadoraID);

            Property(x => x.bordo).HasColumnName("bordo");
            Property(x => x.espaciamiento).HasColumnName("espaciamiento");
            Property(x => x.barrenos).HasColumnName("barrenos");
            Property(x => x.profundidad).HasColumnName("profundidad");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.densidadMaterial).HasColumnName("densidadMaterial");
            Property(x => x.tipoBarreno).HasColumnName("tipoBarreno");
            Property(x => x.subBarreno).HasColumnName("subBarreno");

            Property(x => x.brocaSerie).HasColumnName("brocaSerie");
            Property(x => x.martilloSerie).HasColumnName("martilloSerie");
            Property(x => x.barraSerie).HasColumnName("barraSerie");
            Property(x => x.barraSegundaSerie).HasColumnName("barraSegundaSerie");
            Property(x => x.culataSerie).HasColumnName("culataSerie");
            Property(x => x.portabitSerie).HasColumnName("portabitSerie");
            Property(x => x.cilindroSerie).HasColumnName("cilindroSerie");
            Property(x => x.zancoSerie).HasColumnName("zancoSerie");

            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.rehabilitacion).HasColumnName("rehabilitacion");
            ToTable("tblB_CapturaDiaria");
        }
    }
}
