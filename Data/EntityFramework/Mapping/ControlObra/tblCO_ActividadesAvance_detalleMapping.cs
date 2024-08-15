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
    class tblCO_ActividadesAvance_detalleMapping : EntityTypeConfiguration<tblCO_Actividades_Avance_Detalle>
    {
        public tblCO_ActividadesAvance_detalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");          
            Property(x => x.cantidadAvance).HasColumnName("cantidadAvance");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.acumuladoAnterior).HasColumnName("acumuladoAnterior");
            Property(x => x.acumuladoActual).HasColumnName("acumuladoActual");
            Property(x => x.avancePorcentaje).HasColumnName("avancePorcentaje");
            Property(x => x.avanceAcumuladoPorcentaje).HasColumnName("avanceAcumuladoPorcentaje");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.importeAvanceAnt).HasColumnName("importeAvanceAnt");
            Property(x => x.importeAvancePeriodo).HasColumnName("importeAvancePeriodo");
            Property(x => x.importeAvanceAcumulado).HasColumnName("importeAvanceAcumulado");

            Property(x => x.actividad_id).HasColumnName("actividad_id");
            HasRequired(x => x.actividad).WithMany(x => x.actividadAvances_detalle).HasForeignKey(d => d.actividad_id);

            Property(x => x.actividadAvance_id).HasColumnName("actividadAvance_id");
            HasRequired(x => x.actividadAvance).WithMany(x => x.actividadAvance_detalle).HasForeignKey(d => d.actividadAvance_id);

            ToTable("tblCO_Actividades_Avance_Detalle");
        }
    }
}
