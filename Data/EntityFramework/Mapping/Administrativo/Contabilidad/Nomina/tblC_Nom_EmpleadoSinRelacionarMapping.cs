using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_EmpleadoSinRelacionarMapping : EntityTypeConfiguration<tblC_Nom_EmpleadoSinRelacionar>
    {
        public tblC_Nom_EmpleadoSinRelacionarMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.numeroEmpleado).HasColumnName("numeroEmpleado");
            Property(x => x.nombreEmpleado).HasColumnName("nombreEmpleado");
            Property(x => x.tipoCuentaId).HasColumnName("tipoCuentaId");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.tipoCuenta).WithMany().HasForeignKey(y => y.tipoCuentaId);
            ToTable("tblC_Nom_EmpleadoSinRelacionar");
        }
    }
}
