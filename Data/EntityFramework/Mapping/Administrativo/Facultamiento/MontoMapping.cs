using Core.Entity.Administrativo.Facultamiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Facultamiento
{
    public class MontoMapping : EntityTypeConfiguration<tblFa_CatMonto>
    {
        public MontoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idFacultamiento).HasColumnName("idFacultamiento");
            Property(x => x.idTabla).HasColumnName("idTabla");
            Property(x => x.renglon).HasColumnName("renglon");
            Property(x => x.max).HasColumnName("max");
            Property(x => x.min).HasColumnName("min");
            ToTable("tblFa_CatMonto");
        }
    }
}