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
    public class CapacitacionSeguridadDNCicloTrabajoRegistroMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro>
    {
        public CapacitacionSeguridadDNCicloTrabajoRegistroMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            //Property(x => x.area).HasColumnName("area");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.cicloID).HasColumnName("cicloID");
            Property(x => x.revisor).HasColumnName("revisor");
            Property(x => x.colaborador).HasColumnName("colaborador");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.acredito).HasColumnName("acredito");
            Property(x => x.retroalimentacion).HasColumnName("retroalimentacion");
            Property(x => x.accionRequerida).HasColumnName("accionRequerida");
            Property(x => x.metodo).HasColumnName("metodo");
            Property(x => x.cursoID).HasColumnName("cursoID");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.evaluador).HasColumnName("evaluador");
            Property(x => x.aprobo).HasColumnName("aprobo");
            Property(x => x.comentariosEvaluador).HasColumnName("comentariosEvaluador");
            Property(x => x.observacionesRevisor).HasColumnName("observacionesRevisor");
            Property(x => x.accionesTomadas).HasColumnName("accionesTomadas");
            Property(x => x.observacionesLider).HasColumnName("observacionesLider");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadDNCicloTrabajoRegistro");
        }
    }
}
