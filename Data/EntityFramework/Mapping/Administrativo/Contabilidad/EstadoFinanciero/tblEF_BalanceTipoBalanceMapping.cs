using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BalanceTipoBalanceMapping : EntityTypeConfiguration<tblEF_BalanceTipoBalance>
    {
        public tblEF_BalanceTipoBalanceMapping()
        {
            HasKey(x => x.id);
            ToTable("tblEF_BalanceTipoBalance");
        }
    }
}
