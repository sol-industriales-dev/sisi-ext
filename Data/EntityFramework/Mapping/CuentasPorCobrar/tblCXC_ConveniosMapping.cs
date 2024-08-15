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
    public class tblCXC_ConveniosMapping : EntityTypeConfiguration<tblCXC_Convenios>
    {
        public tblCXC_ConveniosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCXC_Convenios");
        }
    }
}
