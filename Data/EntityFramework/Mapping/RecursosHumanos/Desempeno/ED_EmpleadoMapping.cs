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
    public class ED_EmpleadoMapping : EntityTypeConfiguration<tblRH_ED_Empleado>
    {
        public ED_EmpleadoMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.empleadoID).HasColumnName("empleadoID");
            Property(x => x.jefeID).HasColumnName("jefeID");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.empleadoID);
            HasRequired(x => x.jefe).WithMany().HasForeignKey(y => y.jefeID);
            ToTable("tblRH_ED_CatEstrategia");
        }
    }
}
