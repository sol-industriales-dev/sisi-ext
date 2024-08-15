using Core.Entity.Maquinaria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class HorometroMapping : EntityTypeConfiguration<tblM_CapHorometro>
    {
        public HorometroMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Economico).HasColumnName("Economico");
            Property(x => x.Horometro).HasColumnName("Horometro");
            Property(x => x.HorasTrabajo).HasColumnName("HorasTrabajo");
            Property(x => x.HorometroAcumulado).HasColumnName("HorometroAcumulado");
            Property(x => x.Desfase).HasColumnName("Desfase");
          //  Property(x => x.DesfaseAcumulado).HasColumnName("DesfaseAcumulado");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.Ritmo).HasColumnName("Ritmo");
            Property(x => x.turno).HasColumnName("Turno");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            Property(x => x.folio).HasColumnName("folio");

            ToTable("tblM_CapHorometro");
        }
    }
}
