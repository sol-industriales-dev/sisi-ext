using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceConceptoEmpresaConsolidadoMapping : EntityTypeConfiguration<tblEF_BalanceConceptoEmpresaConsolidado>
    {
        public tblEF_BalanceConceptoEmpresaConsolidadoMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.concepto).WithMany().HasForeignKey(x => x.conceptoId);
            ToTable("tblEF_BalanceConceptoEmpresaConsolidado");
        }
    }
}
