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
    public class CatPipasMapping : EntityTypeConfiguration<tblM_CatPipas>
    {
        public CatPipasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            ToTable("tblM_CatPipas");
        }
    }
}