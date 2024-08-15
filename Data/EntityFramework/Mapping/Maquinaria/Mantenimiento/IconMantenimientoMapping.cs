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
    class IconMantenimientoMapping : EntityTypeConfiguration<tblM_IconMantenimiento>
    {
        public IconMantenimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.icon).HasColumnName("icon");
            Property(x => x.idComp).HasColumnName("idComp");
            ToTable("tblM_IconMantenimiento");
        }
    }
}
