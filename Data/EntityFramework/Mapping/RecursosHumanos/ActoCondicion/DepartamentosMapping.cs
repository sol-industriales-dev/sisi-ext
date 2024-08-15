using Core.Entity.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.ActoCondicion
{
    public class DepartamentosMapping : EntityTypeConfiguration<tblRH_AC_Departamentos>
    {
        public DepartamentosMapping()
        {
            HasKey(x => x.id);
            ToTable("tblRH_AC_Departamentos");
        }
    }
}
