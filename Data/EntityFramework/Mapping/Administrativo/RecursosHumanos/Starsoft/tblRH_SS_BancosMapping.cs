using Core.Entity.Administrativo.RecursosHumanos.Starsoft;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Starsoft
{
    public class tblRH_SS_BancosMapping : EntityTypeConfiguration<tblRH_SS_Bancos>
    {
        public tblRH_SS_BancosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_SS_Bancos");
        }
    }
}
