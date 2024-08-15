using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Tipos_NominaMapping : EntityTypeConfiguration<tblRH_EK_Tipos_Nomina>
    {
        public tblRH_EK_Tipos_NominaMapping()
        {
            HasKey(x => x.tipo_nomina);
            Property(x => x.tipo_nomina).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("tipo_nomina");
            ToTable("tblRH_EK_Tipos_Nomina");
        }
    }
}
