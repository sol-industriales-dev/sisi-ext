using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_REGLA_PuestosMapping : EntityTypeConfiguration<tblRH_BN_REGLA_Puestos>
    {
        public tblRH_BN_REGLA_PuestosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.tipo).HasColumnName("tipo");
            
            ToTable("tblRH_BN_REGLA_Puestos");
        }
    }
}
