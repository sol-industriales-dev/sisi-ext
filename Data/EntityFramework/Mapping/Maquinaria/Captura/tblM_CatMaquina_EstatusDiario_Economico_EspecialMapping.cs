using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class tblM_CatMaquina_EstatusDiario_Economico_EspecialMapping : EntityTypeConfiguration<tblM_CatMaquina_EstatusDiario_Economico_Especial>
    {
        public tblM_CatMaquina_EstatusDiario_Economico_EspecialMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");

            ToTable("tblM_CatMaquina_EstatusDiario_Economico_Especial");
        }
    }
}
