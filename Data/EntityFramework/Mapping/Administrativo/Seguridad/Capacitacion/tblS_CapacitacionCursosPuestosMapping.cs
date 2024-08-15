using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionCursosPuestosMapping : EntityTypeConfiguration<tblS_CapacitacionCursosPuestos>
    {
        public tblS_CapacitacionCursosPuestosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.puesto_id).HasColumnName("puesto_id");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.curso_id).HasColumnName("curso_id");
            HasRequired(x => x.curso).WithMany(x => x.Puestos).HasForeignKey(d => d.curso_id);
            Property(x => x.division).HasColumnName("division");

            ToTable("tblS_CapacitacionCursosPuestos");
        }
    }
}
