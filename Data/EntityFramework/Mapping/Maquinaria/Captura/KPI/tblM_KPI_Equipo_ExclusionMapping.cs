using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.KPI
{
    public class tblM_KPI_Equipo_ExclusionMapping : EntityTypeConfiguration<tblM_KPI_Equipo_Exclusion>
    {
        public tblM_KPI_Equipo_ExclusionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.noEconomico).HasColumnName("noEconomico");

            ToTable("tblM_KPI_Equipo_Exclusion");
        }
    }
}
