using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class MovAlmDetMapping : EntityTypeConfiguration<MovAlmDet>
    {
        public MovAlmDetMapping()
        {
            HasKey(x => new { x.DEALMA, x.DETD, x.DENUMDOC, x.DEITEM });
            ToTable("MovAlmDet");
        }
    }
}
