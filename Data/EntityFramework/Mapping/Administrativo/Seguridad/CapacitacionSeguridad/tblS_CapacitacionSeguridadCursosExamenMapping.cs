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
    public class tblS_CapacitacionSeguridadCursosExamenMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadCursosExamen>
    {
        public tblS_CapacitacionSeguridadCursosExamenMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreExamen).HasColumnName("nombreExamen");
            Property(x => x.pathExamen).HasColumnName("pathExamen");
            Property(x => x.isActivo).HasColumnName("isActivo");
            Property(x => x.tipoExamen).HasColumnName("tipoExamen");


            Property(x => x.curso_id).HasColumnName("curso_id");
            HasRequired(x => x.Cursos).WithMany(x => x.Examenes).HasForeignKey(d => d.curso_id);
            Property(x => x.division).HasColumnName("division");

            ToTable("tblS_CapacitacionSeguridadCursosExamen");
        }

    }
}
