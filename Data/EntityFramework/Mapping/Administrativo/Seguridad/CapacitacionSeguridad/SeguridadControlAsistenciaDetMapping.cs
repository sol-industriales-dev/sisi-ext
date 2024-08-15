using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class SeguridadControlAsistenciaDetMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadControlAsistenciaDetalle>
    {
        public SeguridadControlAsistenciaDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.examenID).HasColumnName("examenID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.estatusAutorizacion).HasColumnName("estatusAutorizacion");
            Property(x => x.rutaExamenInicial).HasColumnName("rutaExamenInicial");
            Property(x => x.rutaExamenFinal).HasColumnName("rutaExamenFinal");
            Property(x => x.rutaDC3).HasColumnName("rutaDC3");
            Property(x => x.controlAsistenciaID).HasColumnName("controlAsistenciaID");
            HasRequired(x => x.controlAsistencia).WithMany(y => y.asistentes).HasForeignKey(x => x.controlAsistenciaID);
            Property(x => x.division).HasColumnName("division");

            ToTable("tblS_CapacitacionSeguridadControlAsistenciaDetalle");
        }
    }

}
