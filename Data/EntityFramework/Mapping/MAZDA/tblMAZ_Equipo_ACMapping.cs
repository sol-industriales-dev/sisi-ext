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
    class tblMAZ_Equipo_ACMapping : EntityTypeConfiguration<tblMAZ_Equipo_AC>
    {
        public tblMAZ_Equipo_ACMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.caracteristicas).HasColumnName("caracteristicas");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.tonelaje).HasColumnName("tonelaje");
            Property(x => x.subAreaID).HasColumnName("subAreaID");
            Property(x => x.subArea).HasColumnName("subArea");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblMAZ_Equipo_AC");
        }
    }
}
