using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class MOVALMCABMapping : EntityTypeConfiguration<MOVALMCAB>
    {
        public MOVALMCABMapping()
        {
            HasKey(x => new { x.CAALMA, x.CATD, x.CANUMDOC });
            ToTable("MOVALMCAB");
        }
    }
}
