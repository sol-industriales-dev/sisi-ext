using Core.Entity.Maquinaria.StandBy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.StandBy
{
    public class tblM_STB_AccionActivacionEconomicoMapping : EntityTypeConfiguration<tblM_STB_AccionActivacionEconomico>
    {
        public tblM_STB_AccionActivacionEconomicoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblM_STB_AccionActivacionEconomico");
        }
    }
}
