using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft
{
    public class FORMA_PAGOMapping : EntityTypeConfiguration<FORMA_PAGO>
    {
        public FORMA_PAGOMapping()
        {
            HasKey(x => x.COD_FP);
            ToTable("FORMA_PAGO");
        }
    }
}
