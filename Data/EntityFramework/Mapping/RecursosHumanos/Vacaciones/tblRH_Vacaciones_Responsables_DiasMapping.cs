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
    class tblRH_Vacaciones_Responsables_DiasMapping : EntityTypeConfiguration<tblRH_Vacaciones_Responsables_Dias>
    {
        public tblRH_Vacaciones_Responsables_DiasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_Vacaciones_Responsables_Dias");
        }
    }
}
