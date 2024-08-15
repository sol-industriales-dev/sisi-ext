using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceCuentaMapping : EntityTypeConfiguration<tblEF_BalanceCuenta>
    {
        public tblEF_BalanceCuentaMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.concepto).WithMany().HasForeignKey(x => x.conceptoId);
            HasRequired(x => x.tipoCuenta).WithMany().HasForeignKey(x => x.tipoCuentaId);
            ToTable("tblEF_BalanceCuenta");
        }
    }
}
