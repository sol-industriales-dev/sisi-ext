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
    class tblMAZ_Reporte_Actividades_EvidenciaMapping : EntityTypeConfiguration<tblMAZ_Reporte_Actividades_Evidencia>
    {
        public tblMAZ_Reporte_Actividades_EvidenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.reporteActividadesID).HasColumnName("reporteActividadesID");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_Reporte_Actividades_Evidencia");
        }
    }
}
