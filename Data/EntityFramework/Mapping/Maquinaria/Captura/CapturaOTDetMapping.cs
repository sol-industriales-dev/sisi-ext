using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class CapturaOTDetMapping : EntityTypeConfiguration<tblM_DetOrdenTrabajo>
    {
        public CapturaOTDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.OrdenTrabajoID).HasColumnName("OrdenTrabajoID");
            Property(x => x.PersonalID).HasColumnName("PersonalID");
            Property(x => x.HorasTrabajo).HasColumnName("HorasTrabajo");
            Property(x => x.HoraInicio).HasColumnName("HoraInicio");
            Property(x => x.HoraFin).HasColumnName("HoraFin");
            Property(x => x.Tipo).HasColumnName("Tipo");
            HasRequired(x => x.OrdenTrabajo).WithMany().HasForeignKey(y => y.OrdenTrabajoID);

            ToTable("tblM_DetOrdenTrabajo");

        }
    }
}
