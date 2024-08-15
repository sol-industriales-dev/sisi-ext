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
    public class ControlAsistenciaMapping : EntityTypeConfiguration<tblS_CapacitacionControlAsistencia>
    {
        public ControlAsistenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cursoID).HasColumnName("cursoID");
            HasRequired(x => x.curso).WithMany().HasForeignKey(x => x.cursoID);
            Property(x => x.instructorID).HasColumnName("instructorID");
            HasRequired(x => x.instructor).WithMany().HasForeignKey(x => x.instructorID);
            Property(x => x.fechaCapacitacion).HasColumnName("fechaCapacitacion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.lugar).HasColumnName("lugar");
            Property(x => x.horario).HasColumnName("horario");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.nombreCarpeta).HasColumnName("nombreCarpeta");
            Property(x => x.rutaListaAsistencia).HasColumnName("rutaListaAsistencia");
            Property(x => x.rutaListaAutorizacion).HasColumnName("rutaListaAutorizacion");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.razonSocial).HasColumnName("razonSocial");
            Property(x => x.esExterno).HasColumnName("esExterno");
            Property(x => x.instructorExterno).HasColumnName("instructorExterno");
            Property(x => x.empresaExterna).HasColumnName("empresaExterna");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuarioCreador).WithMany().HasForeignKey(x => x.usuarioCreadorID);
            Property(x => x.division).HasColumnName("division");
            Property(x => x.validacion).HasColumnName("validacion");
            ToTable("tblS_CapacitacionControlAsistencia");
        }
    }

}