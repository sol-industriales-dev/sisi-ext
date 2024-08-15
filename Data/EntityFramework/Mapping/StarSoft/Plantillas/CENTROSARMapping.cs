using Core.Entity.StarSoft.Plantillas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Plantillas
{
    public class CENTROSARMapping : EntityTypeConfiguration<CENTROSAR>
    {
        public CENTROSARMapping() 
        {
            HasKey(x => x.CODCAR);
            Property(x => x.CODCAR).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CODCAR");
            ToTable("CENTROSAR");
        }
    }
}