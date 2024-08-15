using Core.Entity.StarSoft.Requisiciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Requisiciones
{
    public class COMOVCMapping : EntityTypeConfiguration<COMOVC>
    {
        public COMOVCMapping()
        {
            HasKey(e => e.OC_CNUMORD);
            Property(x => x.OC_CNUMORD).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("OC_CNUMORD");
            ToTable("COMOVC");
        }
    }
}
