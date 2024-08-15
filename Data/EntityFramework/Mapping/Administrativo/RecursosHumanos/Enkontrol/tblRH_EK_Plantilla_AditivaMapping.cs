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
    public class tblRH_EK_Plantilla_AditivaMapping : EntityTypeConfiguration<tblRH_EK_Plantilla_Aditiva>
    {
        public tblRH_EK_Plantilla_AditivaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.virtualPlantilla).WithMany().HasForeignKey(x => x.id_plantilla);
            HasRequired(x => x.virtualPuesto).WithMany().HasForeignKey(x => x.puesto);
            //HasRequired(x => x.virtualSolicita).WithMany().HasForeignKey(x => x.solicita);
            //HasRequired(x => x.virtualVistoBueno).WithMany().HasForeignKey(x => x.visto_bueno);
            //HasRequired(x => x.virtualAutoriza).WithMany().HasForeignKey(x => x.autoriza);
            ToTable("tblRH_EK_Plantilla_Aditiva");
        }
    }
}
