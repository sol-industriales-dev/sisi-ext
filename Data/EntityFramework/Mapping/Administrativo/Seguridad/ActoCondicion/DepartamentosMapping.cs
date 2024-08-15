using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.ActoCondicion
{
    public class DepartamentosMapping : EntityTypeConfiguration<tblSAC_Departamentos>
    {
        public DepartamentosMapping()
        {
            HasKey(x => x.id);
            ToTable("tblSAC_Departamentos");
        }
    }
}
