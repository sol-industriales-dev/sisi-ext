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
    public class tblRH_EK_Plantilla_PuestoMapping : EntityTypeConfiguration<tblRH_EK_Plantilla_Puesto>
    {
        public tblRH_EK_Plantilla_PuestoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.virtualPlantilla).WithMany().HasForeignKey(x => x.id_plantilla);
            HasRequired(x => x.virtualPuesto).WithMany().HasForeignKey(x => x.puesto);
            ToTable("tblRH_EK_Plantilla_Puesto");
        }
    }
}
