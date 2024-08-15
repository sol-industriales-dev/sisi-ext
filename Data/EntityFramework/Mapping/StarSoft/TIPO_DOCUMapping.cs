using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft
{
    public class TIPO_DOCUMapping : EntityTypeConfiguration<TIPO_DOCU>
    {
        public TIPO_DOCUMapping()
        {
            HasKey(x => x.TDO_TIPDOC);
            ToTable("TIPO_DOCU");
        }
    }
}
