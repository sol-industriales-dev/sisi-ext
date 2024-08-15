using Core.Entity.StarSoft;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft
{
    public class TIPO_ARTICULOMapping : EntityTypeConfiguration<TIPO_ARTICULO>
    {
        public TIPO_ARTICULOMapping()
        {
            HasKey(x => x.COD_TIPO);
            ToTable("TIPO_ARTICULO");
        }
    }
}
