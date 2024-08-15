using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Mantenimiento;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class PmMapping : EntityTypeConfiguration<tblM_CatPM>
    {
        public PmMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.PM).HasColumnName("PM");
            Property(x => x.tipoMantenimiento).HasColumnName("tipoMantenimiento");
            ToTable("tblM_catPM");
        }
    }
}
