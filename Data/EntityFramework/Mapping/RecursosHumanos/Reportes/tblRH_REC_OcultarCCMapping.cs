using Core.Entity.RecursosHumanos.Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reportes
{

    public class tblRH_REC_OcultarCCMapping : EntityTypeConfiguration<tblRH_REC_OcultarCC>
    {
        public tblRH_REC_OcultarCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_REC_OcultarCC");
        }
    }
}
