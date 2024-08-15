using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class COMGUICABMapping : EntityTypeConfiguration<COMGUICAB>
    {
        public COMGUICABMapping()
        {
            HasKey(x => new { x.CCTD, x.CCNUMSER, x.CCNUMDOC, x.CCCODPRO });
            ToTable("COMGUICAB");
        }
    }
}
