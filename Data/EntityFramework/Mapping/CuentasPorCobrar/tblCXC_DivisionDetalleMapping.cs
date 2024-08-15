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

    public class tblCXC_DivisionDetalleMapping : EntityTypeConfiguration<tblCXC_DivisionDetalle>
    {
        public tblCXC_DivisionDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.acID).HasColumnName("acID");

            HasRequired(x => x.ac).WithMany().HasForeignKey(d => d.acID);
            ToTable("tblCXC_DivisionDetalle");
        }
    }
}
