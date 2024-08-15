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
    public class tblK_catCcDivMapping : EntityTypeConfiguration<tblK_catCcDiv>
    {
        public tblK_catCcDivMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idDivision).HasColumnName("idDivision");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblK_catCcDiv", "Kubrix");
        }
    }
}
