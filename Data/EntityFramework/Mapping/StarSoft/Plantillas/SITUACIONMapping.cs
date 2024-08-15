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
    public class SITUACIONMapping : EntityTypeConfiguration<SITUACION>
    {
        public SITUACIONMapping() 
        {
            HasKey(x => x.CODIGO);
            Property(x => x.CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CODIGO");
            ToTable("SITUACION");
        }
    }
}