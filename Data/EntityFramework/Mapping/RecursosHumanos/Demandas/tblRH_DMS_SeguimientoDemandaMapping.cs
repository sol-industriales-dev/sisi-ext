using Core.Entity.RecursosHumanos.Demandas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Demandas
{
    public class tblRH_DMS_SeguimientoDemandaMapping : EntityTypeConfiguration<tblRH_DMS_SeguimientoDemanda>
    {
        public tblRH_DMS_SeguimientoDemandaMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_DMS_SeguimientoDemanda");
        }
    }
}
