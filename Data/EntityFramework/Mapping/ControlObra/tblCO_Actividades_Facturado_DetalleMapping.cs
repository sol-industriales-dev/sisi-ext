using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    class tblCO_Actividades_Facturado_DetalleMapping : EntityTypeConfiguration<tblCO_Actividades_Facturado_Detalle>
    {
        public tblCO_Actividades_Facturado_DetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.volumen).HasColumnName("volumen");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.actividad_id).HasColumnName("actividad_id");
            HasRequired(x => x.actividad).WithMany(x => x.actividadFacturado_detalle).HasForeignKey(d => d.actividad_id);
            Property(x => x.actividadFacturado_id).HasColumnName("actividadFacturado_id");
            HasRequired(x => x.facturado).WithMany(x => x.facturado_detalle).HasForeignKey(d => d.actividadFacturado_id);

            ToTable("tblCO_Actividades_Facturado_Detalle");
        }
    }
}
