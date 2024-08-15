using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class ActividadContadorMapping : EntityTypeConfiguration<tblM_ActvContPM>
    {
        public ActividadContadorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Contador).HasColumnName("Contador");
            Property(x => x.idActividad).HasColumnName("idActividad");
            Property(x => x.idMaquina).HasColumnName("idMaquina");
            Property(x => x.idMantenimientoPm).HasColumnName("idMantenimientoPm");
            //Property(x => x.VidaUtil).HasColumnName("VidaUtil");
            Property(x => x.idParteVidaUtil).HasColumnName("idParteVidaUtil");
            ToTable("tblM_ActvContPM");
        }
    }
}
