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
    class ActividadMapping : EntityTypeConfiguration<tblM_CatActividadPM>
    {
        public ActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcionActividad).HasColumnName("descripcionActividad");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.idCatTipoActividad).HasColumnName("idCatTipoActividad");
            ToTable("tblM_CatActividadPM");

        }
    }
}
