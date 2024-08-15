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
    class tblMAZ_Subarea_ReferenciaMapping : EntityTypeConfiguration<tblMAZ_Subarea_Referencia>
    {
        public tblMAZ_Subarea_ReferenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subAreaID).HasColumnName("subAreaID");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_Subarea_Referencia");
        }
    }
}
