using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.MAZDA
{
    class tblMAZ_AreaMapping : EntityTypeConfiguration<tblMAZ_Area>
    {
        public tblMAZ_AreaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.cuadrillaID).HasColumnName("cuadrillaID");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblMAZ_Area");
        }
    }
}
