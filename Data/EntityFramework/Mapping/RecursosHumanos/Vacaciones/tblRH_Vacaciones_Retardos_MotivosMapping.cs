using Core.Entity.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Retardos_MotivosMapping : EntityTypeConfiguration<tblRH_Vacaciones_Retardos_Motivos>
    {
        public tblRH_Vacaciones_Retardos_MotivosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_Vacaciones_Retardos_Motivos");
        }
    }
}
