using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Proveedor
{
    public class MAEPROVMapping : EntityTypeConfiguration<MAEPROV>
    {
        public MAEPROVMapping()
        {
            HasKey(x => x.PRVCCODIGO);
            ToTable("MAEPROV");
        }
    }
}
