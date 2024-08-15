using Core.Entity.SAAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SAAP
{
    class AreaMapping : EntityTypeConfiguration<tblSAAP_Area>
    {
        public AreaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblSAAP_Area");
        }
    }
}
