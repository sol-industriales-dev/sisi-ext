using Core.Entity.Administrativo.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Evaluacion
{
    public class EmpleadoMapping : EntityTypeConfiguration<tblSED_Empleado>
    {
        public EmpleadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apellidoPaterno).HasColumnName("apellidoPaterno");
            Property(x => x.apellidoMaterno).HasColumnName("apellidoMaterno");
            Property(x => x.puestoEvaluacionID).HasColumnName("puestoEvaluacionID");
            Property(x => x.evaluador).HasColumnName("evaluador");
            Property(x => x.rol).HasColumnName("rol");
            Property(x => x.fechaInicioRol).HasColumnName("fechaInicioRol");
            Property(x => x.superUsuario).HasColumnName("superUsuario");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaInicioCC).HasColumnName("fechaInicioCC");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");

            ToTable("tblSED_Empleado");
        }
    }
}
