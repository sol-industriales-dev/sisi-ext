using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class KARDEX_VALMapping : EntityTypeConfiguration<KARDEX_VAL>
    {
        public KARDEX_VALMapping()
        {
            HasKey(x => new { x.ALMACEN, x.NUM_DOC, x.COD_ART, x.TIP_TRANSA});
            ToTable("KARDEX_VAL");
        }
    }
}
