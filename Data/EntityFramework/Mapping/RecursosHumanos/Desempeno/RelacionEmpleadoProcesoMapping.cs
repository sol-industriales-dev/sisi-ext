using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class RelacionEmpleadoProcesoMapping : EntityTypeConfiguration<tblRH_ED_RelacionEmpleadoProceso>
    {
        public RelacionEmpleadoProcesoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ProcesoId).HasColumnName("procesoId");
            Property(x => x.EmpleadoId).HasColumnName("empleadoId");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.Proceso).WithMany().HasForeignKey(y => y.ProcesoId);
            HasRequired(x => x.Empleado).WithMany().HasForeignKey(y => y.EmpleadoId);
            ToTable("tblRH_ED_RelacionEmpleadoProceso");
        }
    }
}