using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_DiasPermitidosMapping : EntityTypeConfiguration<tblRH_REC_DiasPermitidos>
    {
        public tblRH_REC_DiasPermitidosMapping()
        {
            HasKey(x => x.id);
            ToTable("tblRH_REC_DiasPermitidos");
        }
    }
}
