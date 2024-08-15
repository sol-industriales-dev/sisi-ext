using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Plantilla_PersonalMapping : EntityTypeConfiguration<tblRH_EK_Plantilla_Personal>
    {
        public tblRH_EK_Plantilla_PersonalMapping()
        {
            HasKey(x => x.id_plantilla);
            Property(x => x.id_plantilla).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id_plantilla");
            ToTable("tblRH_EK_Plantilla_Personal");
        }
    }
}
