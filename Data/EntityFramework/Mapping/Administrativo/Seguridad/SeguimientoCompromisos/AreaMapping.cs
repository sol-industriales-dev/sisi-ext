using Core.Entity.SeguimientoCompromisos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SeguimientoCompromisos
{
    class AreaMapping : EntityTypeConfiguration<tblSC_Area>
    {
        public AreaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblSC_Area");
        }
    }
}
