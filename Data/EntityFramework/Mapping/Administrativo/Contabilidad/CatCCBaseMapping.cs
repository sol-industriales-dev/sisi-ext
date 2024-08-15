using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class CatCCBaseMapping : EntityTypeConfiguration<tblC_CatCCBase>
    {
        public CatCCBaseMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.centro_costos).HasColumnName("centro_costos");
            Property(x => x.nombCC).HasColumnName("nombCC");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.total).HasColumnName("total");
            ToTable("tblC_CatCCBase");
        }
    }
}
