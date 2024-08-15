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
    public class AUDITORIA_SISTEMASMapping : EntityTypeConfiguration<AUDITORIA_SISTEMAS>
    {
        public AUDITORIA_SISTEMASMapping()
        {
            HasKey(x => x.SECUENCIA);
            Property(x => x.SECUENCIA).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("SECUENCIA");
            ToTable("AUDITORIA_SISTEMAS");
        }
    }
}
