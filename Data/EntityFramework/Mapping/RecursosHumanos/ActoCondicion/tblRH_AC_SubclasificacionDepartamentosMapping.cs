using Core.Entity.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.ActoCondicion
{
    public class tblRH_AC_SubclasificacionDepartamentosMapping : EntityTypeConfiguration<tblRH_AC_SubclasificacionDepartamentos>
    {
        public tblRH_AC_SubclasificacionDepartamentosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subclasificacionDep).HasColumnName("subclasificacionDep");
            Property(x => x.idDepartamento).HasColumnName("idDepartamento");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblRH_AC_SubclasificacionDepartamentos");
        }
    }
}
