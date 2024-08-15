using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_EstadosMapping : EntityTypeConfiguration<tblRH_EK_Estados>
    {
        public tblRH_EK_EstadosMapping()
        {
            HasKey(x => x.id);
            ToTable("tblRH_EK_Estados");
        }
    }
}
