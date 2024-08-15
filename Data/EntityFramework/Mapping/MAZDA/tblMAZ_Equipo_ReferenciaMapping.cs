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
    class tblMAZ_Equipo_ReferenciaMapping : EntityTypeConfiguration<tblMAZ_Equipo_Referencia>
    {
        public tblMAZ_Equipo_ReferenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.equipoID).HasColumnName("areaID");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_Equipo_Referencia");
        }
    }
}
