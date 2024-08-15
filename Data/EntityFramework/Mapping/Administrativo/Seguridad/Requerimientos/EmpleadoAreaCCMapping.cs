using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Requerimientos
{
    public class EmpleadoAreaCCMapping : EntityTypeConfiguration<tblS_Req_EmpleadoAreaCC>
    {
        public EmpleadoAreaCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            Property(x => x.esContratista).HasColumnName("esContratista");

            ToTable("tblS_Req_EmpleadoAreaCC");
        }
    }
}
