using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_EmpleadosMapping : EntityTypeConfiguration<tblRH_EK_Empleados>
    {
        public tblRH_EK_EmpleadosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasOptional(x => x.requisicionEntity).WithMany().HasForeignKey(x => x.requisicion);
            ToTable("tblRH_EK_Empleados");
        }
    }
}
