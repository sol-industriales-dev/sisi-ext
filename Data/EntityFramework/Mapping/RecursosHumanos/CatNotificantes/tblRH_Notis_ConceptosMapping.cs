using Core.Entity.RecursosHumanos.CatNotificantes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.CatNotificantes
{
    public class tblRH_Notis_ConceptosMapping : EntityTypeConfiguration<tblRH_Notis_Conceptos>
    {

        public tblRH_Notis_ConceptosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_Notis_Conceptos");
        }
    }
}
