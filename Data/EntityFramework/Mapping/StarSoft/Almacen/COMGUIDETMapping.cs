using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class COMGUIDETMapping : EntityTypeConfiguration<COMGUIDET>
    {
        public COMGUIDETMapping()
        {
            HasKey(x => new { x.DCTD, x.DCNUMSER, x.DCNUMDOC, x.DCCODPRO, x.DCITEM });
            ToTable("COMGUIDET");
        }
    }
}
