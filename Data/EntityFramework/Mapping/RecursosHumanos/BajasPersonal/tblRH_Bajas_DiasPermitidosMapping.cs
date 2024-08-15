using Core.Entity.RecursosHumanos.BajasPersonal;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.BajasPersonal
{
    public class tblRH_Bajas_DiasPermitidosMapping : EntityTypeConfiguration<tblRH_Bajas_DiasPermitidos>
    {
        public tblRH_Bajas_DiasPermitidosMapping()
        {
            HasKey(x => x.id);
            ToTable("tblRH_Bajas_DiasPermitidos");
        }
    }
}
