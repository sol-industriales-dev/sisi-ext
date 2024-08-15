using Core.Entity.RecursosHumanos.Bajas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Bajas
{
    public class tblRH_Baja_Entrevista_ConceptosMapping : EntityTypeConfiguration<tblRH_Baja_Entrevista_Conceptos>
    {
        public tblRH_Baja_Entrevista_ConceptosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.preguntaID).HasColumnName("preguntaID");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblRH_Baja_Entrevista_Conceptos");
        }
    }
}
