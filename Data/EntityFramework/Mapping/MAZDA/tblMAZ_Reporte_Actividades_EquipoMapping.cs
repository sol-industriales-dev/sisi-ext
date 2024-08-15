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
    class tblMAZ_Reporte_Actividades_EquipoMapping : EntityTypeConfiguration<tblMAZ_Reporte_Actividades_Equipo>
    {
        public tblMAZ_Reporte_Actividades_EquipoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.reporteActividadesID).HasColumnName("reporteActividadesID");
            Property(x => x.equipoID).HasColumnName("equipoID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_Reporte_Actividades_Equipo");
        }
    }
}
