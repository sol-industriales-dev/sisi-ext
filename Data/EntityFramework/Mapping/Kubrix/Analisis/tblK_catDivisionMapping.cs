using Core.Entity.Kubrix.Analisis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Kubrix.Analisis
{
    public class tblK_catDivisionMapping : EntityTypeConfiguration<tblK_catDivision>
    {
        public tblK_catDivisionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Divsion).HasColumnName("Divsion");
            ToTable("tblK_catDivision", "Kubrix");
        }
    }
}
