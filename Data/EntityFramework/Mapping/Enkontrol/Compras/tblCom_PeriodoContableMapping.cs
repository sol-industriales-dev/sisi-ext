using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras
{
    class tblCom_PeriodoContableMapping : EntityTypeConfiguration<tblCom_PeriodoContable>
    {
        public tblCom_PeriodoContableMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.soc).HasColumnName("soc");
            Property(x => x.sfa).HasColumnName("sfa");

            ToTable("tblCom_PeriodoContable");
        }
    }
}
