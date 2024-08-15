using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft
{
    public class MAECLIMapping : EntityTypeConfiguration<MAECLI>
    {
        public MAECLIMapping()
        {
            HasKey(x => x.CCODCLI);
            ToTable("MAECLI");
        }
    }
}
