using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Proveedor
{
    public class CUENTA_CORRIENTE_PROVMapping : EntityTypeConfiguration<CUENTA_CORRIENTE_PROV>
    {
        public CUENTA_CORRIENTE_PROVMapping()
        {
            HasKey(x => x.ANEXO);
            ToTable("CUENTA_CORRIENTE_PROV");
        }
    }
}
