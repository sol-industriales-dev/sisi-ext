using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.MAZDA
{
    class tblMAZ_Usuario_ActividadMapping : EntityTypeConfiguration<tblMAZ_Usuario_Actividad>
    {
        public tblMAZ_Usuario_ActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioCuadrillaID).HasColumnName("usuarioCuadrillaID");
            Property(x => x.actividadPeriodoID).HasColumnName("actividadPeriodoID");
            ToTable("tblMAZ_Usuario_Actividad");
        }
    }
}
