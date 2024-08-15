using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.Conciliacion
{
    public class EncCaratula_CatConsideracionCostoHora : EntityTypeConfiguration<tblM_EncCaratula_Concideracion>
    {
        public EncCaratula_CatConsideracionCostoHora()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.EncCaratula).HasColumnName("EncCaratula");
            Property(x => x.ConsideracionCostoHora).HasColumnName("ConsideracionCostoHora");
            Property(x => x.isActivo).HasColumnName("isActivo");
            ToTable("tblM_EncCaratulatblM_CatConsideracionCostoHora");
        }
    }
}
