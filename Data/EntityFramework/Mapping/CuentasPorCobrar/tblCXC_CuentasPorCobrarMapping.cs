using Core.Entity.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.CuentasPorCobrar
{

    public class tblCXC_CuentasPorCobrarMapping : EntityTypeConfiguration<tblCXC_CuentasPorCobrar>
    {
        public tblCXC_CuentasPorCobrarMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCXC_CuentasPorCobrar");
        }
    }
}
