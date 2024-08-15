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
    class tblMAZ_ActividadMapping : EntityTypeConfiguration<tblMAZ_Actividad>
    {
        public tblMAZ_ActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.detalle).HasColumnName("detalle");
            Property(x => x.areaID).HasColumnName("areaID");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblMAZ_Actividad");
        }
    }
}
