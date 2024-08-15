using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceGrupoConsolidadoMapping : EntityTypeConfiguration<tblEF_BalanceGrupoConsolidado>
    {
        public tblEF_BalanceGrupoConsolidadoMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.tipoBalance).WithMany().HasForeignKey(x => x.tipoBalanceId);
            ToTable("tblEF_BalanceGrupoConsolidado");
        }
    }
}
