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
    public class tblRH_BN_Incidencia_ConceptoMapping : EntityTypeConfiguration<tblRH_BN_Incidencia_Concepto>
    {
        public tblRH_BN_Incidencia_ConceptoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");

            ToTable("tblRH_BN_Incidencia_Concepto");
        }
    }
}
