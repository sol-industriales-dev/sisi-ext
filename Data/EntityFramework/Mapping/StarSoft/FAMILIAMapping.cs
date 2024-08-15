using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft
{
    public class FAMILIAMapping : EntityTypeConfiguration<FAMILIA>
    {
        public FAMILIAMapping()
        {
            HasKey(x => x.FAM_CODIGO);
            ToTable("FAMILIA");
        }
    }
}
