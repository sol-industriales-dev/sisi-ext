using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class RitmoHorometroMapping: EntityTypeConfiguration<tblM_CapRitmoHorometro>
    {
        public RitmoHorometroMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.horasDiarias).HasColumnName("horasDiarias");
            Property(x => x.horasSemana).HasColumnName("horasSemana");

            ToTable("tblM_CapRitmoHorometro");
        }
    }
}
