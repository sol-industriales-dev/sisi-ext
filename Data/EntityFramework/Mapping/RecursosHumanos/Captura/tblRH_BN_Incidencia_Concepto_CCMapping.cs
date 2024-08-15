using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencia_Concepto_CCMapping : EntityTypeConfiguration<tblRH_BN_Incidencia_Concepto_CC>
    {
        public tblRH_BN_Incidencia_Concepto_CCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.conceptoID).HasColumnName("conceptoID");
            Property(x => x.cc).HasColumnName("cc");

            ToTable("tblRH_BN_Incidencia_Concepto_CC");
        }
    }
}
