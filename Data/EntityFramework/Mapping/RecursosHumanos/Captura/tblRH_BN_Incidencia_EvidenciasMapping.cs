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
    public class tblRH_BN_Incidencia_EvidenciasMapping : EntityTypeConfiguration<tblRH_BN_Incidencia_Evidencias>
    {
        public tblRH_BN_Incidencia_EvidenciasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_BN_Incidencia_Evidencias");
        }       
    }
}
