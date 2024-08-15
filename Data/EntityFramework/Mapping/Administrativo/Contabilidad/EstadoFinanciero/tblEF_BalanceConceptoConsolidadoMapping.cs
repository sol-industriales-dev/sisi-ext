using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceConceptoConsolidadoMapping : EntityTypeConfiguration<tblEF_BalanceConceptoConsolidado>
    {
        public tblEF_BalanceConceptoConsolidadoMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.grupo).WithMany().HasForeignKey(x => x.grupoId);
            ToTable("tblEF_BalanceConceptoConsolidado");
        }
    }
}
