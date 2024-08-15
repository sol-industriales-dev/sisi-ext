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
    public class CapacitacionSeguridadCursosCCMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadCursosCC>
    {
        public CapacitacionSeguridadCursosCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.curso_id).HasColumnName("curso_id");
            HasRequired(x => x.Curso).WithMany(x => x.CentrosCosto).HasForeignKey(d => d.curso_id);
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadCursosCC");
        }
    }
}
