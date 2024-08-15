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
    class tblMAZ_Reporte_ActividadesMapping : EntityTypeConfiguration<tblMAZ_Reporte_Actividades>
    {
        public tblMAZ_Reporte_ActividadesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.revisionTipo).HasColumnName("revisionTipo");
            Property(x => x.revisionDetalleID).HasColumnName("revisionDetalleID");
            Property(x => x.ultMant).HasColumnName("ultMant");
            Property(x => x.sigMant).HasColumnName("sigMant");
            Property(x => x.descripcionActividad).HasColumnName("descripcionActividad");
            Property(x => x.semaforo).HasColumnName("semaforo");
            Property(x => x.reprogramacion).HasColumnName("reprogramacion");
            Property(x => x.fechaReprogramacion).HasColumnName("fechaReprogramacion");
            Property(x => x.estatusInfo).HasColumnName("estatusInfo");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_Reporte_Actividades");
        }
    }
}
