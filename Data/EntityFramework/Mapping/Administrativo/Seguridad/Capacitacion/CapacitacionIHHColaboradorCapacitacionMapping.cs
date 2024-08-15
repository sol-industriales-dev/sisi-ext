using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class CapacitacionIHHColaboradorCapacitacionMapping : EntityTypeConfiguration<tblS_CapacitacionIHHColaboradorCapacitacion>
    {
        public CapacitacionIHHColaboradorCapacitacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.equipo).HasColumnName("equipo");
            Property(x => x.equipoAdiestramiento_id).HasColumnName("equipoAdiestramiento_id");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaTermino).HasColumnName("fechaTermino");
            Property(x => x.colaborador).HasColumnName("colaborador");
            Property(x => x.adiestrador).HasColumnName("adiestrador");
            Property(x => x.instructor).HasColumnName("instructor");
            Property(x => x.seguridad).HasColumnName("seguridad");
            Property(x => x.recursosHumanos).HasColumnName("recursosHumanos");
            Property(x => x.sobrestante).HasColumnName("sobrestante");
            Property(x => x.gerenteObra).HasColumnName("gerenteObra");
            Property(x => x.liberado).HasColumnName("liberado");
            Property(x => x.rutaSoporteAdiestramiento).HasColumnName("rutaSoporteAdiestramiento");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionIHHColaboradorCapacitacion");
        }
    }
}
