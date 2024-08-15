using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft
{
    public class TIPO_CAMBIOMapping : EntityTypeConfiguration<TIPO_CAMBIO>
    {
        public TIPO_CAMBIOMapping()
        {
            HasKey(x => x.TIPOCAMB_FECHA);
            ToTable("TIPO_CAMBIO");
        }
    }
}
